using MongoDB.Driver;
using super_ticketing_backend.Models;

namespace super_ticketing_backend.Repositories;

public class TicketRepository : ITicketRepository
{
    private readonly IMongoCollection<Tickets> _ticketsCollection;

    public TicketRepository(IMongoCollection<Tickets> ticketsCollection)
    {
        _ticketsCollection = ticketsCollection;
    }

    public async Task<List<Tickets>> GetAsync() =>
        await _ticketsCollection.Find(_ => true).ToListAsync();

    public async Task<Tickets?> GetAsync(string id) =>
        await _ticketsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Tickets newTicket) =>
        await _ticketsCollection.InsertOneAsync(newTicket);

    public async Task UpdateAsync(string id, Tickets updateTicket) =>
        await _ticketsCollection.ReplaceOneAsync(x => x.Id == id, updateTicket);

    public async Task RemoveAsync(string id) =>
        await _ticketsCollection.DeleteOneAsync(x => x.Id == id);
}