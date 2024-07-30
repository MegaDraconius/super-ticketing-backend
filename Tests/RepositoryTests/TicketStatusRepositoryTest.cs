using Microsoft.Build.Framework;
using Mongo2Go;
using MongoDB.Bson;
using MongoDB.Driver;
using super_ticketing_backend.Models;
using super_ticketing_backend.Repositories;

namespace Tests;

public class TicketStatusRepositoryTest : IDisposable
{
    private readonly MongoDbRunner _runner;
    private readonly IMongoCollection<TicketStatus> _ticketStatusCollection;
    private readonly TicketStatusRepository _ticketStatusRepository;
    private readonly IMongoDatabase _database;

    public TicketStatusRepositoryTest()
    {
        _runner = MongoDbRunner.Start();
        var client = new MongoClient(_runner.ConnectionString);
        _database = client.GetDatabase("TestDatabase");
        _ticketStatusCollection = _database.GetCollection<TicketStatus>("TicketStatus");
        _ticketStatusRepository = new TicketStatusRepository(_ticketStatusCollection);

        var ticketStatus = new TicketStatus
        {
            Id = ObjectId.GenerateNewId().ToString(),
            StatusValue = "Pendiente"
        };

        _ticketStatusCollection.InsertOne(ticketStatus);
    }

    [Fact]
    public async Task GetAsync_ReturnAllTicketStatus()
    {
        var ticketStatus = await _ticketStatusRepository.GetAsync();
        
        Assert.NotEmpty(ticketStatus);
    }

    [Fact]
    public async Task CreateAsync_AddNewTicketStatus()
    {
        var newTicketStatus = new TicketStatus
        {
            Id = ObjectId.GenerateNewId().ToString(),
            StatusValue = "En curso"
        };

        await _ticketStatusRepository.CreateAsync(newTicketStatus);

        var ticketStatus = await _ticketStatusRepository.GetAsync();

        Assert.Contains(ticketStatus, u => u.Id == newTicketStatus.Id);
    }

    [Fact]
    public async Task UpdateAsync_UpdateExistingTicketStatus()
    {
        // Obtiene el valor del ticketStatus inicial
        var ticketStatus = await _ticketStatusCollection.Find(_ => true).FirstOrDefaultAsync();
        Assert.NotNull(ticketStatus); 

        // Cambia el StatusValue
        ticketStatus.StatusValue = "Resuelto";

        // Actualiza el TicketStatus en la base de datos
        await _ticketStatusRepository.UpdateAsync(ticketStatus);

        //obtiene el ticket actualizado
        var updatedTicketStatus = await _ticketStatusCollection.Find(x => x.Id == ticketStatus.Id).FirstOrDefaultAsync();

        
        Assert.NotNull(updatedTicketStatus);  
        Assert.Equal("Resuelto", updatedTicketStatus.StatusValue);
        }
    
    public void Dispose()
    {
        _runner.Dispose();
    }
}

    