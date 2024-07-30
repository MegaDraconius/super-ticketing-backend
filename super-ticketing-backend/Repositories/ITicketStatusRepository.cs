using super_ticketing_backend.Models;

namespace super_ticketing_backend.Repositories;

public interface ITicketStatusRepository
{
    Task<List<TicketStatus>> GetAsync();

    Task CreateAsync(TicketStatus newTicketStatus);

    Task UpdateAsync(TicketStatus updatedTicketStatus);
    
}