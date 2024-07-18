using super_ticketing_backend.Models;

namespace super_ticketing_backend.Repositories;

public interface IITGuyRepository
{
    Task<List<ITGuys>> GetAsync();
    Task<ITGuys?> GetAsync(string id);
    Task CreateAsync(ITGuys newItGuy);
    Task UpdateAsync(string id, ITGuys updatedItGuy);
    Task RemoveAsync(string id);
}