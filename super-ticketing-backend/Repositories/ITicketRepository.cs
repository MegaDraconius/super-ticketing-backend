using super_ticketing_backend.Models;

namespace super_ticketing_backend.Repositories;

public interface ITicketRepository
{
    Task<List<Tickets>> GetAsync();
    Task<Tickets?> GetAsync(string id);
    Task CreateAsync(Tickets newTicket);
    Task UpdateAsync(string id, Tickets updateTicket);
    Task RemoveAsync(string id);
}