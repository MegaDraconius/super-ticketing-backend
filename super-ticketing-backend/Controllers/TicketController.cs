using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using super_ticketing_backend.Dto_s;
using super_ticketing_backend.Models;
using super_ticketing_backend.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;

using super_ticketing_backend.Services.MailingService;
using super_ticketing_backend.Services.PhotoService;


namespace super_ticketing_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserRepository _userRepository;
        private readonly IITGuyRepository _itGuyRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMailingSystem _mailingSystem;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;


        public TicketController(ITicketRepository ticketRepository, IUserRepository userRepository, IITGuyRepository itGuyRepository, ICountryRepository countryRepository, IMailingSystem mailingSystem, IMapper mapper, IPhotoService photoService)
        {
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
            _itGuyRepository = itGuyRepository;
            _countryRepository = countryRepository;
            _mailingSystem = mailingSystem;
            _mapper = mapper;
            _photoService = photoService;
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

        [HttpGet("{id}")]
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

                var countryId = user.CountryId;
                var countryData = await _countryRepository.GetAsync(countryId);
                var countryCode = countryData.CountryCode;

                var splitId = ticketDto.Id.Substring(0, 8);

                var trackingId = $"{countryCode}-{splitId}";

                //ticketDto.TrackingId = trackingId;
                
                var userEmail = ticketDto.UserEmail;
                var about = ticketDto.Title;
                //var trackingId = ticketDto.TrackingId;

                await _ticketRepository.UpdateTrackingId(ticketDto.Id, trackingId);
                await _mailingSystem.SendCreationMail(userEmail, about, trackingId);
                // await _ticketRepository.SendMail(userEmail, about);
                
                return CreatedAtAction(nameof(Get), new { id = ticketDto.Id,  }, ticketDto);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, TicketUpdateDto updatedTicketDto)
        {
            var ticket = await _ticketRepository.GetAsync(id);

            if (ticket is null)
            {
                return NotFound();
            }

            var itGuy = await _itGuyRepository.GetByEmailAsync(updatedTicketDto.ItGuyEmail);
            if (itGuy != null)
            {
                ticket.ITGuyId = itGuy.Id;
            }

            var oldStatus = ticket.Status;
            
            if (updatedTicketDto.Status == "Resuelto")
            {
                var currentDate = DateTime.Now;
                ticket.SolvedDate = currentDate;
            }

            _mapper.Map(updatedTicketDto, ticket);
            await _ticketRepository.UpdateAsync(ticket);

            var ticketDto = _mapper.Map<TicketDto>(ticket);
            
            var user = await _userRepository.GetByIdAsync(ticket.UserId);
            if (user != null)
            {
                ticketDto.UserEmail = user.UserEmail;
            }
            //ticketDto.UserEmail = user?.UserEmail;
            ticketDto.ItGuyEmail = itGuy?.ItGuyEmail;
            
            if (updatedTicketDto.Status == "Resuelto")
            {
                await _mailingSystem.SendStatusUpdateMail(ticketDto.UserEmail, "Incidencia resuelta",
                    updatedTicketDto.Status, ticketDto.Title, ticketDto.Feedback);
            }

            return Ok();
        }

        [HttpPatch("{id:length(24)}/status")]
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
        
        [HttpPatch("{id:length(24)}/stored")]
        public async Task<ActionResult> ChangeStored(string id, bool storedValue)
        {
            var ticket = await _ticketRepository.GetAsync(id);
            
            if (ticket == null)
            {
                return NotFound();
            }
        
            await _ticketRepository.UpdateStored(id, storedValue);
        
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
