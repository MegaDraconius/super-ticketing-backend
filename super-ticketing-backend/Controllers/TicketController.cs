using Microsoft.AspNetCore.Mvc;
using super_ticketing_backend.Models;
using super_ticketing_backend.Repositories;

namespace super_ticketing_backend.Controllers;

[ApiController]
[Route("api/[controller]")]

public class TicketController : ControllerBase
{
    private readonly ITicketRepository _ticketRepository;
    
    public TicketController(ITicketRepository ticketRepository) =>
        _ticketRepository = ticketRepository;

    [HttpGet]
    public async Task<List<Tickets>> Get() =>
        await _ticketRepository.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Tickets>> Get(string id)
    {
        var ticket = await _ticketRepository.GetAsync(id);
        
        if (ticket is null)
        {
            return NotFound();
        }

        return ticket;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Tickets newTicket)
    {
        await _ticketRepository.CreateAsync(newTicket);
        
        return CreatedAtAction(nameof(Get), new { id = newTicket.Id }, newTicket);
    }
    
    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Tickets updatedTicket)
    {
        var ticket = await _ticketRepository.GetAsync(id);

        if (ticket is null)
        {
            return NotFound();
        }

        updatedTicket.Id = ticket.Id;

        await _ticketRepository.UpdateAsync(id, updatedTicket);

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