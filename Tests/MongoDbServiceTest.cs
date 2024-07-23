using MongoDB.Driver;
using Xunit;
using Microsoft.Extensions.Configuration;
using super_ticketing_backend.DataBaseSettings;
using super_ticketing_backend.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Tests
{
    public class MongoDbConnectionTest
    {
        private readonly IConfiguration _configuration;

        public MongoDbConnectionTest()
        {
            var inMemorySettings = new Dictionary<string, string> {
                {"super-ticketing-backend:ConnectionString", "mongodb+srv://rogeresteve5:JiQLT0VAtZryqrJF@ticketing.dt8mqku.mongodb.net/"},
                {"super-ticketing-backend:DatabaseName", "Ticketing"},
                {"super-ticketing-backend:UsersCollectionName", "User"},
                {"super-ticketing-backend:ITGuysCollectionName", "ITEmployees"},
                {"super-ticketing-backend:CountryCollectionName", "Country"},
                {"super-ticketing-backend:TicketsCollectionName", "Ticket"}
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }

        [Fact]
        public void TestMongoDbConnection()
        {
            var services = new ServiceCollection();
            services.AddMongoDb(_configuration);

            var serviceProvider = services.BuildServiceProvider();
            var mongoClient = serviceProvider.GetRequiredService<MongoClient>();
            var database = serviceProvider.GetRequiredService<IMongoDatabase>();

            Assert.NotNull(mongoClient);
            Assert.NotNull(database);

            var collectionNames = database.ListCollectionNames().ToList();
            Assert.NotEmpty(collectionNames);
        }
    }
}