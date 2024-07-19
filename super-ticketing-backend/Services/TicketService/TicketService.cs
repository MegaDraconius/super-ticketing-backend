using Microsoft.Extensions.Options;
using MongoDB.Driver;
using super_ticketing_backend.Models;

namespace super_ticketing_backend.Services.TicketService;

public class TicketService
{
    private readonly IMongoCollection<Tickets> _ticketsCollection;
    
    public TicketService(
        IOptions<DataBaseSettings.DataBaseSettings> ticketsStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            ticketsStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            ticketsStoreDatabaseSettings.Value.DatabaseName);

        _ticketsCollection = mongoDatabase.GetCollection<Tickets>(
            ticketsStoreDatabaseSettings.Value.TicketsCollectionName);
    }
    public IMongoCollection<Tickets> GetTikcetsCollection() => _ticketsCollection;
}