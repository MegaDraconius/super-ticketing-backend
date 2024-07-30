using MongoDB.Driver;
using super_ticketing_backend.Models;

namespace super_ticketing_backend.Repositories;

public class ITGuyRepository : IITGuyRepository
{
    private readonly IMongoCollection<ITGuys> _itGuysCollection;
    
    public ITGuyRepository(IMongoCollection<ITGuys> itGuysCollection)
    {
        _itGuysCollection = itGuysCollection;
    }
    
    public async Task<List<ITGuys>> GetAsync() =>
        await _itGuysCollection.Find(_ => true).ToListAsync();

    public async Task<ITGuys?> GetAsync(string id) =>
        await _itGuysCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(ITGuys newItGuy) =>
        await _itGuysCollection.InsertOneAsync(newItGuy);
    
    public async Task<ITGuys?> GetByEmailAsync(string email) =>
        await _itGuysCollection.Find(x => x.ItGuyEmail == email).FirstOrDefaultAsync();

    public async Task UpdateAsync(ITGuys updatedItGuy) =>
        await _itGuysCollection.ReplaceOneAsync(x => x.Id == updatedItGuy.Id, updatedItGuy);

    public async Task RemoveAsync(string id) =>
        await _itGuysCollection.DeleteOneAsync(x => x.Id == id);
}