using MongoDB.Driver;
using super_ticketing_backend.Models;

namespace super_ticketing_backend.Repositories;

public class TicketStatusRepository : ITicketStatusRepository
{
    private readonly IMongoCollection<TicketStatus> _ticketStatusCollection;

    public TicketStatusRepository(IMongoCollection<TicketStatus> ticketStatusCollection)
    {
        _ticketStatusCollection = ticketStatusCollection;
    }
    
    public async Task<List<TicketStatus>> GetAsync() =>
        await _ticketStatusCollection.Find(_ => true).ToListAsync();
    
    public async Task CreateAsync(TicketStatus newTicketStatus) =>
        await _ticketStatusCollection.InsertOneAsync(newTicketStatus);
    
    public async Task UpdateAsync(TicketStatus updatedTicketStatus) =>
        await _ticketStatusCollection.ReplaceOneAsync(x => x.Id == updatedTicketStatus.Id, updatedTicketStatus);
}