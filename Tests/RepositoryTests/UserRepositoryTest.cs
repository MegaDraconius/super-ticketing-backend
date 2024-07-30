using Microsoft.AspNetCore.Identity.Data;
using Mongo2Go;
using MongoDB.Bson;
using MongoDB.Driver;
using super_ticketing_backend.Dto_s;
using super_ticketing_backend.Models;
using super_ticketing_backend.Repositories;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    public class UserRepositoryTests : IDisposable
    {
        private readonly MongoDbRunner _runner;
        private readonly IMongoCollection<Users> _usersCollection;
        private readonly UserRepository _userRepository;
        private readonly IMongoDatabase _database;

        public UserRepositoryTests(ITestOutputHelper output)
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
                CountryId = ObjectId.GenerateNewId().ToString(),
                UserEmail = "test@example.com",
                Pwd = "Hello1!",
                Role = "Admin",
                AccessToken = Guid.NewGuid().ToString("N")
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
            Assert.NotNull(result);
            Assert.Equal(user.Id, result.Id);
        }

        [Fact]
        public async Task GetByEmailAsync_WithValidEmail_ReturnUser()
        {
            // Arrange
            var user = await _usersCollection.Find(_ => true).FirstOrDefaultAsync();

            // Act
            var result = await _userRepository.GetByEmailAsync(user.UserEmail);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.UserEmail, result.UserEmail);
        }

        [Fact]
        public async Task CreateAsync_AddNewUser()
        {
            // Arrange
            var newUser = new Users
            {
                Id = ObjectId.GenerateNewId().ToString(),
                CountryId = ObjectId.GenerateNewId().ToString(),
                UserEmail = "test@test.com",
                Pwd = "PassWord",
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
            user.UserEmail = "hello@hello.com";

            // Act
            await _userRepository.UpdateAsync(user);
            var updatedUser = await _userRepository.GetAsync(user.Id);

            // Assert
            Assert.NotNull(updatedUser);
            Assert.Equal("hello@hello.com", updatedUser.UserEmail);
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
        
        [Fact]
        public async Task Login_WithValidCredentials_ReturnsUser()
        {
            // Arrange
            var user = await _usersCollection.Find(_ => true).FirstOrDefaultAsync();
            var loginRequest = new LoginRequestDto
            {
                UserEmail = user.UserEmail,
                Pwd = user.Pwd
            };

            // Act
            var result = await _userRepository.Login(loginRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(loginRequest.UserEmail, result.UserEmail);
            Assert.Equal(user.Role, result.Role);
            Assert.Equal(user.AccessToken, result.AccessToken);
        }
        
        [Fact]
        public async Task UpdateAccessToken_UpdatesUserToken()
        {
            // Arrange
            var user = await _usersCollection.Find(_ => true).FirstOrDefaultAsync();
            var newToken = Guid.NewGuid().ToString("N");

            // Act
            await _userRepository.UpdateAccessToken(user.Id, newToken);
            var updatedUser = await _userRepository.GetAsync(user.Id);

            // Assert
            Assert.NotNull(updatedUser);
            Assert.Equal(newToken, updatedUser.AccessToken);
        }


        [Fact]
        public async Task Login_WithInvalidCredentials_ReturnsNull()
        {
            // Arrange
            var user = await _usersCollection.Find(_ => true).FirstOrDefaultAsync();
            var loginRequest = new LoginRequestDto
            {
                UserEmail = user.UserEmail,
                Pwd = "ContraseñaIncorrecta123"
            };

            // Act
            var result = await _userRepository.Login(loginRequest);

            // Assert
            Assert.Null(result);
        }

        public void Dispose()
        {
            _runner.Dispose();
        }
    }
}
