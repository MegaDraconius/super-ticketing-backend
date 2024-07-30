// using Mongo2Go;
// using MongoDB.Driver;
// using MongoDB.Bson;
// using super_ticketing_backend.Models;
// using super_ticketing_backend.Repositories;
//
// namespace Tests;
//
// public class TicketRepositoryTest : IDisposable
// {
//     private readonly MongoDbRunner _runner;
//
//     private readonly IMongoCollection<Tickets> _ticketCollection;
//
//     private readonly IMongoCollection<ITGuys> _itGuyCollection;
//     private readonly IMongoCollection<Users> _userCollection;
//     private readonly IMongoCollection<Country> _countryCollection;
//     private readonly TicketRepository _ticketRepository;
//     private readonly IMongoDatabase _database;
//
//
//     public TicketRepositoryTest()
//     {
//         _runner = MongoDbRunner.Start();
//         var client = new MongoClient(_runner.ConnectionString);
//         _database = client.GetDatabase("TestDatabase");
//         _ticketCollection = _database.GetCollection<Tickets>("Ticket");
//         _userCollection = _database.GetCollection<Users>("User");
//         _itGuyCollection = _database.GetCollection<ITGuys>("ITEmployees");
//         _countryCollection = _database.GetCollection<Country>("Country");
//         _ticketRepository = new TicketRepository(_ticketCollection);
//
//
//         var itGuy = new ITGuys
//         {
//             Id = ObjectId.GenerateNewId().ToString(),
//             ItGuyName = "Matheus",
//             Surname = "Da Sousa",
//             Pwd = "Hello1!",
//             Role = "Administrator",
//             CountryId = "Spain",
//             ItGuyEmail = "matheusdasousa@gmail.com"
//         };
//         _itGuyCollection.InsertOne(itGuy);
//
//         var user = new Users
//         {
//             Id = ObjectId.GenerateNewId().ToString(),
//             CountryId = "Portugal",
//             UserEmail = "test@test.com",
//             Pwd = "Hello1!",
//             Role = "Tech"
//         };
//         _userCollection.InsertOne(user);
//
//         var country = new Country
//         {
//             Id = ObjectId.GenerateNewId().ToString(),
//             CountryCode = "POR",
//             CountryName = "Portugal",
//             CountryMainLanguage = "Portuguese"
//         };
//         _countryCollection.InsertOne(country);
//
//         var ticket = new Tickets
//         {
//             Id = ObjectId.GenerateNewId().ToString(),
//             TrackingId = "POR000000001",
//             Title = "Ordenador roto",
//             Description = "El ordenador se me ha roto",
//             ReportDate = DateTime.Now,
//             Status = "Open",
//             Country = country.Id,
//             Priority = "High",
//             UserId = user.Id,
//             ITGuyId = itGuy.Id
//         };
//         _ticketCollection.InsertOne(ticket);
//     }
//
//     [Fact]
//     public async Task GetAsync_ReturnAllTickets()
//     {
//         var tickets = await _ticketRepository.GetAsync();
//         
//         Assert.NotEmpty(tickets);
//     }
//
//     [Fact]
//     public async Task GetAsync_WithValidId_ReturnTicket()
//     {
//         var ticket = await _ticketCollection.Find(_ => true).FirstOrDefaultAsync();
//         var result = await _ticketRepository.GetAsync(ticket.Id);
//         Assert.NotNull(result);
//     }
//
//     [Fact]
//     public async Task GetAsync_NotNull_ReturnTicket()
//     {
//         var ticket = await _ticketCollection.Find(_ => true).FirstOrDefaultAsync();
//         var result = await _ticketRepository.GetAsync(ticket.Id);
//
//         Assert.NotNull(result);
//     }
//
//     [Fact]
//     public async Task CreateAsync_AddNewTicket()
//     {
//         var newTicket = new Tickets()
//         {
//             Id = ObjectId.GenerateNewId().ToString(),
//             TrackingId = "POR000000001",
//             Title = "Ordenador roto",
//             Description = "El ordenador se me ha roto",
//             ReportDate = DateTime.Now,
//             Status = "Open",
//             Country = (await _countryCollection.Find(_ => true).FirstOrDefaultAsync()).Id,
//             Priority = "High",
//             UserId = (await _userCollection.Find(_ => true).FirstOrDefaultAsync()).Id,
//             ITGuyId = (await _itGuyCollection.Find(_ => true).FirstOrDefaultAsync()).Id
//         };
//
//         await _ticketRepository.CreateAsync(newTicket);
//
//         var ticket = await _ticketRepository.GetAsync();
//
//         Assert.Contains(ticket, u => u.Id == newTicket.Id);
//     }
//
//     [Fact]
//         public async Task UpdateAsync_UpdateExistingTicket()
//         {
//             var ticket = await _ticketCollection.Find(_ => true).FirstOrDefaultAsync();
//             ticket.Priority = "Low";
//
//             await _ticketRepository.UpdateAsync(ticket);
//             var updatedTicket = await _ticketRepository.GetAsync(ticket.Id);
//
//             Assert.Equal("Low", updatedTicket.Priority);
//         }
//         
//         [Fact]
//         public async Task RemoveAsync_DeleteTicket()
//         {
//             var ticket = await _ticketCollection.Find(_ => true).FirstOrDefaultAsync();
//
//             await _ticketRepository.RemoveAsync(ticket.Id);
//             var result = await _ticketRepository.GetAsync(ticket.Id);
//             
//             Assert.Null(result);
//
//         }
//     public void Dispose()
//         {
//             _runner.Dispose();
//         }
// }
