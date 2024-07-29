using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using super_ticketing_backend.Dto_s;
using super_ticketing_backend.Models;
using super_ticketing_backend.Repositories;

namespace super_ticketing_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketStatusController : ControllerBase
    {
        private readonly ITicketStatusRepository _ticketStatusRepository;
        private readonly IMapper _mapper;

        public TicketStatusController(ITicketStatusRepository ticketStatusRepository, IMapper mapper)
        {
            _ticketStatusRepository = ticketStatusRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<List<TicketStatusDto>> GetTicketStatus()
        {
            var ticketStatus = await _ticketStatusRepository.GetAsync();
            var ticketStatusDto = _mapper.Map<List<TicketStatusDto>>(ticketStatus);
            
            return ticketStatusDto;
        }

        [HttpPost]
        public async Task<ActionResult> CreateTicketStatus(TicketStatusCreateDto ticketStatusCreateDto)
        {
            try
            {
                var newTicketStatus = _mapper.Map<TicketStatus>(ticketStatusCreateDto);
                await _ticketStatusRepository.CreateAsync(newTicketStatus);
                
                var ticketDto = _mapper.Map<TicketStatusDto>(newTicketStatus);
                
                return CreatedAtAction(nameof(GetTicketStatus), new { id = ticketDto.Id }, ticketDto);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    
}
