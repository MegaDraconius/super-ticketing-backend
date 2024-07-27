using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using super_ticketing_backend.Dto_s;
using super_ticketing_backend.Models;
using super_ticketing_backend.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;
using super_ticketing_backend.Services.MailingService;

namespace super_ticketing_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserRepository _userRepository;
        private readonly IITGuyRepository _itGuyRepository;
        private readonly IMailingSystem _mailingSystem;
        private readonly IMapper _mapper;

        public TicketController(ITicketRepository ticketRepository, IUserRepository userRepository, IITGuyRepository itGuyRepository, IMailingSystem mailingSystem, IMapper mapper)
        {
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
            _itGuyRepository = itGuyRepository;
            _mailingSystem = mailingSystem;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<List<TicketDto>> Get()
        {
            var tickets = await _ticketRepository.GetAsync();
            var ticketDtos = _mapper.Map<List<TicketDto>>(tickets);

            foreach (var ticketDto in ticketDtos)
            {
                var user = await _userRepository.GetAsync(ticketDto.UserId);
                var itGuy = await _itGuyRepository.GetAsync(ticketDto.ItGuyId);

                ticketDto.UserEmail = user?.UserEmail;
                ticketDto.ItGuyEmail = itGuy?.ItGuyEmail;
            }

            return ticketDtos;
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<TicketDto>> Get(string id)
        {
            var ticket = await _ticketRepository.GetAsync(id);

            if (ticket is null)
            {
                return NotFound();
            }

            var ticketDto = _mapper.Map<TicketDto>(ticket);

            var user = await _userRepository.GetAsync(ticketDto.UserId);
            var itGuy = await _itGuyRepository.GetAsync(ticketDto.ItGuyId);

            ticketDto.UserEmail = user?.UserEmail;
            ticketDto.ItGuyEmail = itGuy?.ItGuyEmail;

            return ticketDto;
        }

        [HttpPost]
        public async Task<IActionResult> Post(TicketCreateDto ticketCreateDto)
        {
            try
            {
                var newTicket = _mapper.Map<Tickets>(ticketCreateDto);
                await _ticketRepository.CreateAsync(newTicket);
                
                var ticketDto = _mapper.Map<TicketDto>(newTicket);
                
                var user = await _userRepository.GetAsync(ticketDto.UserId);
                ticketDto.UserEmail = user?.UserEmail;
                
                var userEmail = ticketDto.UserEmail;
                var about = ticketDto.Title;
                var trackingId = ticketDto.TrackingId;

                await _mailingSystem.SendCreationMail(userEmail, about, trackingId);
                // await _ticketRepository.SendMail(userEmail, about);
                
                return CreatedAtAction(nameof(Get), new { id = ticketDto.Id }, ticketDto);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, TicketUpdateDto updatedTicketDto)
        {
            var ticket = await _ticketRepository.GetAsync(id);

            if (ticket is null)
            {
                return NotFound();
            }

            var user = await _userRepository.GetByEmailAsync(updatedTicketDto.UserEmail);
            if (user != null)
            {
                ticket.UserId = user.Id;
            }

            var itGuy = await _itGuyRepository.GetByEmailAsync(updatedTicketDto.ItGuyEmail);
            if (itGuy != null)
            {
                ticket.ITGuyId = itGuy.Id;
            }

            

            _mapper.Map(updatedTicketDto, ticket);
            await _ticketRepository.UpdateAsync(ticket);

            var ticketDto = _mapper.Map<TicketDto>(ticket);
            ticketDto.UserEmail = user?.UserEmail;
            ticketDto.ItGuyEmail = itGuy?.ItGuyEmail;
            
            if (ticket.Status != updatedTicketDto.Status)
            {
                await _mailingSystem.SendStatusUpdateMail(ticketDto.UserEmail, "Cambio estado de incidencia",
                    updatedTicketDto.Status);
            }

            return NoContent();
        }

        [HttpPatch("{id:length(24)}")]
        public async Task<ActionResult> ChangeStatus(string id, string statusValue)
        {
            var ticket = await _ticketRepository.GetAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            await _ticketRepository.UpdateStatus(id, statusValue);

            return Ok();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var ticket = await _ticketRepository.GetAsync(id);

            if (ticket is null)
            {
                return NotFound();
            }

            await _ticketRepository.RemoveAsync(id);

            return NoContent();
        }
    }
}
