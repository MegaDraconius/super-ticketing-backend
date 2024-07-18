using Microsoft.Extensions.Options;
using MongoDB.Driver;
using super_ticketing_backend.Models;

namespace super_ticketing_backend.Services.UserService;


public class UserService
{
    private readonly IMongoCollection<Users> _usersCollection;
    
    public UserService(
        IOptions<DataBaseSettings.DataBaseSettings> userStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            userStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            userStoreDatabaseSettings.Value.DatabaseName);

        _usersCollection = mongoDatabase.GetCollection<Users>(
            userStoreDatabaseSettings.Value.UsersCollectionName);
    }
    public IMongoCollection<Users> GetUsersCollection() => _usersCollection;
}