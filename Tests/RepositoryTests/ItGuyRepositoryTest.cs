using Mongo2Go;
using MongoDB.Bson;
using MongoDB.Driver;
using super_ticketing_backend.Models;
using super_ticketing_backend.Repositories;

namespace Tests;

public class ItGuyRepositoryTest: IDisposable
{
    private readonly MongoDbRunner _runner;
    private readonly IMongoCollection<ITGuys> _itGuyCollection;
    private readonly ITGuyRepository _itGuyRepository;
    private readonly IMongoDatabase _database;
    

    public ItGuyRepositoryTest()
    {
        _runner= MongoDbRunner.Start();
        var client = new MongoClient(_runner.ConnectionString);
        _database = client.GetDatabase("TestDatabase");
        _itGuyCollection = _database.GetCollection<ITGuys>("ItGuys");
        _itGuyRepository = new ITGuyRepository(_itGuyCollection);

        var itGuy = new ITGuys
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Name = "Roger",
            Surname = "Esteve",
            Pwd = "Hello1!",
            Role = "Administrator",
            Country = "Spain",
            Email = "rogeresteve@gmail.com"
        };
        _itGuyCollection.InsertOne(itGuy);
        }

        [Fact]
        public async Task GetAsync_ReturnAllItGuy()
        {
            var itGuys = await _itGuyRepository.GetAsync();
            
            Assert.NotEmpty(itGuys);
        }

        [Fact]
        public async Task GetAsync_NotNull_ReturnItGuy()
        {
            var itGuy = await _itGuyCollection.Find(_ => true).FirstOrDefaultAsync();

            var result = await _itGuyRepository.GetAsync(itGuy.Id);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetAsync_WithValidId_ReturnItGuy()
        {
            var itGuy = await _itGuyCollection.Find(_ => true).FirstOrDefaultAsync();
            
            var result = await _itGuyRepository.GetAsync(itGuy.Id);
            
            Assert.Equal(itGuy.Id, result.Id);
        }

        [Fact]
        public async Task CreateAsync_AddNewItGuy()
        {
            var newiTGuy = new ITGuys
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Sara",
                Surname = "Jorja",
                Pwd = "Hello1!",
                Role = "Administrator",
                Country = "Spain",
                Email = "sarajorja@gmail.com"
            };

            await _itGuyRepository.CreateAsync(newiTGuy);

            var itGuys = await _itGuyRepository.GetAsync();

            Assert.Contains(itGuys, u => u.Id == newiTGuy.Id);
        }

        [Fact]
        public async Task UpdateAsync_UpdateExsitingUser()
        {
            var itGuy = await _itGuyCollection.Find(_ => true).FirstOrDefaultAsync();
            itGuy.Name = "Alejandro";

            await _itGuyRepository.UpdateAsync(itGuy);
            var updatedItGuy = await _itGuyRepository.GetAsync(itGuy.Id);
            
            Assert.Equal("Alejandro", updatedItGuy.Name);
        }

        [Fact]
        public async Task RemoveAsync_DeleteItGuy()
        {
            var itGuy = await _itGuyCollection.Find(_ => true).FirstOrDefaultAsync();

            await _itGuyRepository.RemoveAsync(itGuy.Id);
            var result = await _itGuyRepository.GetAsync(itGuy.Id);
            
            Assert.Null(result);
        }

        public void Dispose()
        {
            _runner.Dispose();
        }
    }


























