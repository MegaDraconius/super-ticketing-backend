using Mongo2Go;
using MongoDB.Bson;
using MongoDB.Driver;
using super_ticketing_backend.Models;
using super_ticketing_backend.Repositories;
using Xunit;

namespace Tests;

    public class UserRepositoryTests : IDisposable
    {
        private readonly MongoDbRunner _runner;
        private readonly IMongoCollection<Users> _usersCollection;
        private readonly UserRepository _userRepository;
        private readonly IMongoDatabase _database;

        public UserRepositoryTests()
        {
            // Iniciar Mongo2Go
            _runner = MongoDbRunner.Start();
            var client = new MongoClient(_runner.ConnectionString);
            _database = client.GetDatabase("TestDatabase");
            _usersCollection = _database.GetCollection<Users>("User");
            _userRepository = new UserRepository(_usersCollection);

            // Sembrar datos de prueba
            var user = new Users
            {
                Id = ObjectId.GenerateNewId().ToString(),
                CountryId = "TestCountry",
                UserEmail = "test@example.com",
                Pwd = "Hello1!",
                Role = "Admin"
            };
            _usersCollection.InsertOne(user);
        }

        [Fact]
        public async Task GetAsync_ReturnAllUsers()
        {
            // Act
            var users = await _userRepository.GetAsync();

            // Assert
            Assert.NotEmpty(users);
        }

        [Fact]
        public async Task GetAsync_WithValidId_ReturnUser()
        {
            // Arrange
            var user = await _usersCollection.Find(_ => true).FirstOrDefaultAsync();

            // Act
            var result = await _userRepository.GetAsync(user.Id);

            // Assert
            // Assert.NotNull(result);
            Assert.Equal(user.Id, result.Id);
        }

        [Fact]
        public async Task GetAsync_NotNull_ReturnUser()
        {
            var user = await _usersCollection.Find(_ => true).FirstOrDefaultAsync();

            // Act
            var result = await _userRepository.GetAsync(user.Id);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CreateAsync_AddNewUser()
        {
            // Arrange
            var newUser = new Users
            {
                Id = ObjectId.GenerateNewId().ToString(),
                CountryId = "NewCountry",
                UserEmail = "new@example.com",
                Pwd = "newpassword",
                Role = "User"
            };

            // Act
            await _userRepository.CreateAsync(newUser);
            var users = await _userRepository.GetAsync();

            // Assert
            Assert.Contains(users, u => u.Id == newUser.Id);
        }

        [Fact]
        public async Task UpdateAsync_UpdateExistingUser()
        {
            // Arrange
            var user = await _usersCollection.Find(_ => true).FirstOrDefaultAsync();
            user.CountryId = "UpdatedCountry";

            // Act
            await _userRepository.UpdateAsync(user);
            var updatedUser = await _userRepository.GetAsync(user.Id);

            // Assert
            Assert.Equal("UpdatedCountry", updatedUser.CountryId);
        }

        [Fact]
        public async Task RemoveAsync_DeleteUser()
        {
            // Arrange
            var user = await _usersCollection.Find(_ => true).FirstOrDefaultAsync();

            // Act
            await _userRepository.RemoveAsync(user.Id);
            var result = await _userRepository.GetAsync(user.Id);

            // Assert
            Assert.Null(result);
        }

        public void Dispose()
        {
            _runner.Dispose();
        }
    }

