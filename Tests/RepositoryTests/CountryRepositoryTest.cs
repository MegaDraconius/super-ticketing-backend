using Mongo2Go;
using MongoDB.Bson;
using MongoDB.Driver;
using super_ticketing_backend.Models;
using super_ticketing_backend.Repositories;

namespace Tests;

public class CountryRepositoryTest : IDisposable
{
    private readonly MongoDbRunner _runner;
    private readonly IMongoCollection<Country> _countryCollection;
    private readonly CountryRepository _countryRepository;
    private readonly IMongoDatabase _database;

    public CountryRepositoryTest()
    {
        _runner = MongoDbRunner.Start();
        var client = new MongoClient(_runner.ConnectionString);
        _database = client.GetDatabase("TestDatabase");
        _countryCollection = _database.GetCollection<Country>("Country");
        _countryRepository = new CountryRepository(_countryCollection);

        var country = new Country
        {
            Id = ObjectId.GenerateNewId().ToString(),
            CountryCode = "POR",
            CountryName = "Portugal",
            CountryMainLanguage = "Portuguese"
        };
        _countryCollection.InsertOne(country);
    }

    [Fact]
    public async Task GetAsync_ReturnAllContry()
    {
        var countries = await _countryRepository.GetAsync();
        
        Assert.NotEmpty(countries);
    }

    [Fact]
    public async Task GetAsync_NotNull_ReturnCountry()
    {
        var country = await _countryCollection.Find(_ => true).FirstOrDefaultAsync();

        var result = await _countryRepository.GetAsync(country.Id);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task CreateAsync_AddNewCountry()
    {
        var newCountry = new Country
        {
            Id = ObjectId.GenerateNewId().ToString(),
            CountryCode = "PO",
            CountryName = "Polonia",
            CountryMainLanguage = "Polish"
        };

        await _countryRepository.CreateAsync(newCountry);

        var countries = await _countryRepository.GetAsync();

        Assert.Contains(countries, u => u.Id == newCountry.Id);
    }

    [Fact]
    public async Task UpdateAsync_UpdateExistingCountry()
    {
        var country = await _countryCollection.Find(_ => true).FirstOrDefaultAsync();
        country.CountryName = "Poland";

        await _countryRepository.UpdateAsync(country);
        var updatedCountry = await _countryRepository.GetAsync(country.Id);
        
        Assert.Equal("Poland", updatedCountry.CountryName);
    }

    [Fact]
    public async Task RemoveAsync_DeleteCountry()
    {
        var country = await _countryCollection.Find(_ => true).FirstOrDefaultAsync();

        await _countryRepository.RemoveAsync(country.Id);
        var result = await _countryRepository.GetAsync(country.Id);
        
        Assert.Null(result);
    }
    
    public void Dispose()
    {
        _runner.Dispose();
    }
}



















