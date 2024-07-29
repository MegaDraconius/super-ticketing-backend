using MongoDB.Driver;
using super_ticketing_backend.Models;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

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

    public async Task UpdateAsync(Tickets updatedTicket) =>
        await _ticketsCollection.ReplaceOneAsync(x => x.Id == updatedTicket.Id, updatedTicket);

    public async Task RemoveAsync(string id) =>
        await _ticketsCollection.DeleteOneAsync(x => x.Id == id);

    public async Task UpdateStatus(string id, string statusValue)
    {
        var filter = Builders<Tickets>.Filter.Eq(ticket => ticket.Id, id);
        var updateValue = Builders<Tickets>.Update.Set(ticket => ticket.Status, statusValue);
        await _ticketsCollection.UpdateOneAsync(filter, updateValue);
    }
    
    // public async Task<List<Tickets>> GetLastTicket() => 
    //     await _ticketsCollection.Find(x => x.TrackingId.Contains("ES%")).ToListAsync();
}

