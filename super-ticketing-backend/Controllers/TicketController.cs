using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using super_ticketing_backend.Dto_s;
using super_ticketing_backend.Models;
using super_ticketing_backend.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace super_ticketing_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserRepository _userRepository;
        private readonly IITGuyRepository _itGuyRepository;
        private readonly IMapper _mapper;

        public TicketController(
            ITicketRepository ticketRepository,
            IUserRepository userRepository,
            IITGuyRepository itGuyRepository,
            IMapper mapper)
        {
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
            _itGuyRepository = itGuyRepository;
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
            var newTicket = _mapper.Map<Tickets>(ticketCreateDto);
            await _ticketRepository.CreateAsync(newTicket);

            var ticketDto = _mapper.Map<TicketDto>(newTicket);

            var user = await _userRepository.GetAsync(ticketDto.UserId);
            ticketDto.UserEmail = user?.UserEmail;

            return CreatedAtAction(nameof(Get), new { id = ticketDto.Id }, ticketDto);
        }

        // [HttpPut("{id:length(24)}")]
        // public async Task<IActionResult> Update(string id, TicketUpdateDto updatedTicketDto)
        // {
        //     var ticket = await _ticketRepository.GetAsync(id);
        //
        //     if (ticket is null)
        //     {
        //         return NotFound();
        //     }
        //
        //     _mapper.Map(updatedTicketDto, ticket);
        //     await _ticketRepository.UpdateAsync(ticket);
        //
        //     var user = await _userRepository.GetAsync(ticket.UserId);
        //     var itGuy = await _itGuyRepository.GetAsync(ticket.ITGuyId);
        //
        //     var ticketDto = _mapper.Map<TicketDto>(ticket);
        //     ticketDto.UserEmail = user?.UserEmail;
        //     ticketDto.ItGuyEmail = itGuy?.ItGuyEmail;
        //
        //     return NoContent();
        // }
        
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

            return NoContent();
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
