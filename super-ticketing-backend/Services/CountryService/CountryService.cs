using Microsoft.Extensions.Options;
using MongoDB.Driver;
using super_ticketing_backend.Models;

namespace super_ticketing_backend.Services.CountryService;

public class CountryService
{
    private readonly IMongoCollection<Country> _countryCollection;
    
    public CountryService(
        IOptions<DataBaseSettings.DataBaseSettings> countryStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            countryStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            countryStoreDatabaseSettings.Value.DatabaseName);

        _countryCollection = mongoDatabase.GetCollection<Country>(
            countryStoreDatabaseSettings.Value.CountryCollectionName);
    }
    public IMongoCollection<Country> GetCountriesCollection() => _countryCollection;
}