using super_ticketing_backend.Dto_s;
using super_ticketing_backend.Models;
namespace super_ticketing_backend.Repositories;
public interface IUserRepository
{
    Task<List<Users>> GetAsync();
    Task<Users?> GetAsync(string id);
    Task CreateAsync(Users newUser);
    Task UpdateAsync(Users updatedUser);
    Task RemoveAsync(string id);
    Task<Users?> GetByIdAsync(string id);
    Task UpdateAccessToken(string id, string newToken);
    Task<Users?> Login(LoginRequestDto loginRequestDto);
}