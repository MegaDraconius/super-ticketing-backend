using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using super_ticketing_backend.Dto_s;
using super_ticketing_backend.Models;
using super_ticketing_backend.Repositories;

namespace super_ticketing_backend.Controllers;

[ApiController]
[Route("api/[controller]")]

public class TicketController : ControllerBase
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IMapper _mapper;

    public TicketController(ITicketRepository ticketRepository, IMapper mapper)
    {
        _ticketRepository = ticketRepository;
        _mapper = mapper;
    }
        

    [HttpGet]
    public async Task<List<TicketDto>> Get()
    {
        var tickets = await _ticketRepository.GetAsync();
        
        return _mapper.Map<List<TicketDto>>(tickets);

        
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
        return ticketDto;
    }

    [HttpPost]
    public async Task<IActionResult> Post(TicketCreateDto ticketCreateDto)
    {
        var newticket = _mapper.Map<Tickets>(ticketCreateDto);
        await _ticketRepository.CreateAsync(newticket);

        var ticketDto = _mapper.Map<TicketDto>(newticket);
        return CreatedAtAction(nameof(Get), new { id = ticketDto.Id }, ticketDto);
    }
    
    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, TicketCreateDto updatedTicketDto)
    {
        var ticket = await _ticketRepository.GetAsync(id);

        if (ticket is null)
        {
            return NotFound();
        }

        _mapper.Map(updatedTicketDto, ticket);
        await _ticketRepository.UpdateAsync(ticket);

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