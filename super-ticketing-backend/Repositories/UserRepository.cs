using MongoDB.Driver;
using super_ticketing_backend.Models;

namespace super_ticketing_backend.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<Users> _usersCollection;
    
    public UserRepository(IMongoCollection<Users> usersCollection)
    {
        _usersCollection = usersCollection;
    }
    
    public async Task<List<Users>> GetAsync() =>
        await _usersCollection.Find(_ => true).ToListAsync();

    public async Task<Users?> GetAsync(string id) =>
        await _usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Users newUser) =>
        await _usersCollection.InsertOneAsync(newUser);

    public async Task UpdateAsync(Users updatedUser) =>
        await _usersCollection.ReplaceOneAsync(x => x.Id == updatedUser.Id, updatedUser);

    public async Task RemoveAsync(string id) =>
        await _usersCollection.DeleteOneAsync(x => x.Id == id);
}