using super_ticketing_backend.Models;

namespace super_ticketing_backend.Repositories;

public interface ICountryRepository
{
    Task<List<Country>> GetAsync();
    Task<Country?> GetAsync(string id);
}