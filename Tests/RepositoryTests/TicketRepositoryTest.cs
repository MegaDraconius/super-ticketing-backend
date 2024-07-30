using Mongo2Go;
using MongoDB.Driver;
using MongoDB.Bson;
using super_ticketing_backend.Models;
using super_ticketing_backend.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class TicketRepositoryTest : IDisposable
    {
        private readonly MongoDbRunner _runner;
        private readonly IMongoCollection<Tickets> _ticketCollection;
        private readonly IMongoCollection<Users> _userCollection;
        private readonly IMongoCollection<ITGuys> _itGuyCollection;
        private readonly IMongoCollection<Country> _countryCollection;
        private readonly TicketRepository _ticketRepository;
        private readonly IMongoDatabase _database;

        public TicketRepositoryTest()
        {
            _runner = MongoDbRunner.Start();
            var client = new MongoClient(_runner.ConnectionString);
            _database = client.GetDatabase("TestDatabase");
            _ticketCollection = _database.GetCollection<Tickets>("Ticket");
            _userCollection = _database.GetCollection<Users>("User");
            _itGuyCollection = _database.GetCollection<ITGuys>("ITGuy");
            _countryCollection = _database.GetCollection<Country>("Country");
            _ticketRepository = new TicketRepository(_ticketCollection);

            // Insert sample Country
            var country = new Country
            {
                Id = ObjectId.GenerateNewId().ToString(),
                CountryCode = "POR",
                CountryName = "Portugal",
                CountryMainLanguage = "Portuguese"
            };
            _countryCollection.InsertOne(country);

            // Insert sample User
            var user = new Users
            {
                Id = ObjectId.GenerateNewId().ToString(),
                CountryId = country.Id,
                UserEmail = "test@test.com",
                Pwd = "Hello1!",
                Role = "Admin"
            };
            _userCollection.InsertOne(user);

            // Insert sample ITGuy
            var itGuy = new ITGuys
            {
                Id = ObjectId.GenerateNewId().ToString(),
                ItGuyName = "Matheus",
                Surname = "Da Sousa",
                Pwd = "Hello1!",
                Role = "ItGuy",
                CountryId = country.Id,
                ItGuyEmail = "matheusdasousa@gmail.com"
            };
            _itGuyCollection.InsertOne(itGuy);

            // Insert sample Ticket
            var ticket = new Tickets
            {
                Id = ObjectId.GenerateNewId().ToString(),
                TrackingId = "POR-000000001",
                Title = "Ordenador roto",
                Description = "El ordenador se me ha roto",
                ReportDate = DateTime.Now,
                Status = "Pendiente",
                Country = country.Id,
                Priority = "8",
                UserId = user.Id,
                ITGuyId = itGuy.Id
            };
            _ticketCollection.InsertOne(ticket);
        }

        [Fact]
        public async Task GetAsync_ReturnsAllTickets()
        {
            var tickets = await _ticketRepository.GetAsync();
            Assert.NotEmpty(tickets);
        }

        [Fact]
        public async Task GetAsync_WithValidId_ReturnsTicket()
        {
            var ticket = await _ticketCollection.Find(_ => true).FirstOrDefaultAsync();
            var result = await _ticketRepository.GetAsync(ticket.Id);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CreateAsync_AddsNewTicket()
        {
            var newTicket = new Tickets
            {
                Id = ObjectId.GenerateNewId().ToString(),
                TrackingId = "POR000000002",
                Title = "No puedo abrir la puerta de la oficina",
                Description = "Debe haber un probelma con la app Tedee porque no me logra conectar el bluetooth",
                ReportDate = DateTime.Now,
                Status = "Resuelto",
                Country = (await _countryCollection.Find(_ => true).FirstOrDefaultAsync()).Id,
                Priority = "8",
                UserId = (await _userCollection.Find(_ => true).FirstOrDefaultAsync()).Id,
                ITGuyId = (await _itGuyCollection.Find(_ => true).FirstOrDefaultAsync()).Id
            };

            await _ticketRepository.CreateAsync(newTicket);

            var tickets = await _ticketRepository.GetAsync();
            Assert.Contains(tickets, t => t.Id == newTicket.Id);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesExistingTicket()
        {
            var ticket = await _ticketCollection.Find(_ => true).FirstOrDefaultAsync();
            ticket.Priority = "6";

            await _ticketRepository.UpdateAsync(ticket);
            var updatedTicket = await _ticketRepository.GetAsync(ticket.Id);

            Assert.Equal("6", updatedTicket.Priority);
        }

        [Fact]
        public async Task RemoveAsync_DeletesTicket()
        {
            var ticket = await _ticketCollection.Find(_ => true).FirstOrDefaultAsync();
            await _ticketRepository.RemoveAsync(ticket.Id);

            var result = await _ticketRepository.GetAsync(ticket.Id);
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateStatus_UpdatesTicketStatus()
        {
            var ticket = await _ticketCollection.Find(_ => true).FirstOrDefaultAsync();
            var newStatus = "Resuelto";

            await _ticketRepository.UpdateStatus(ticket.Id, newStatus);
            var updatedTicket = await _ticketRepository.GetAsync(ticket.Id);

            Assert.Equal(newStatus, updatedTicket.Status);
        }

        [Fact]
        public async Task UpdateStored_UpdatesTicketStored()
        {
            var ticket = await _ticketCollection.Find(_ => true).FirstOrDefaultAsync();
            var newStoredValue = true;

            await _ticketRepository.UpdateStored(ticket.Id, newStoredValue);
            var updatedTicket = await _ticketRepository.GetAsync(ticket.Id);

            Assert.Equal(newStoredValue, updatedTicket.Stored);
        }

        public void Dispose()
        {
            _runner.Dispose();
        }
    }
}
