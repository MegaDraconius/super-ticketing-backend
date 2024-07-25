using super_ticketing_backend.Models;

namespace super_ticketing_backend.Repositories;

public interface IITGuyRepository
{
    Task<List<ITGuys>> GetAsync();
    Task<ITGuys?> GetAsync(string id);
    Task CreateAsync(ITGuys newItGuy);
    Task UpdateAsync(ITGuys updateditGuy);
    Task RemoveAsync(string id);
    Task<ITGuys?> GetByEmailAsync(string email);
    
}