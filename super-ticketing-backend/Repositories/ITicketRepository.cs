using super_ticketing_backend.Models;

namespace super_ticketing_backend.Repositories;

public interface ITicketRepository
{
    Task<List<Tickets>> GetAsync();
    Task<Tickets?> GetAsync(string id);
    Task CreateAsync(Tickets newTicket);
    Task UpdateAsync(Tickets updatedTicket);
    Task RemoveAsync(string id);
    // Task SendMail(string to, string about);

    Task UpdateStatus(string id, string statusValue);
    
    Task UpdateStored(string id, bool storedValue);


    // Task<List<Tickets>> GetLastTicket();
}