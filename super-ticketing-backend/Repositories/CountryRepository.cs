using MongoDB.Driver;
using super_ticketing_backend.Models;

namespace super_ticketing_backend.Repositories;

public class CountryRepository : ICountryRepository
{
    private readonly IMongoCollection<Country> _countryCollection;

    public CountryRepository(IMongoCollection<Country> countryCollection)
    {
        _countryCollection = countryCollection;
    }

    public async Task<List<Country>> GetAsync() =>
        await _countryCollection.Find(_ => true).ToListAsync();

    public async Task<Country?> GetAsync(string id) =>
        await _countryCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
}