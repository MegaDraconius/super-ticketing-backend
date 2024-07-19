using super_ticketing_backend.Models;

namespace super_ticketing_backend.Repositories;

public interface ICountryRepository
{
    Task<List<Country>> GetAsync();
    Task<Country?> GetAsync(string id);
    Task CreateAsync(Country newCountry);
    Task UpdateAsync(string id, Country updateCountry);
    Task RemoveAsync(string id);
}