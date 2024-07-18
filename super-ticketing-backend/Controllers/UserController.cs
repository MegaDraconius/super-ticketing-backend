using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using super_ticketing_backend.Models;
using super_ticketing_backend.Repositories;
using super_ticketing_backend.Services.UserService;

namespace super_ticketing_backend.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UserController : ControllerBase
{
    
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository usersRepository) =>
        _userRepository = usersRepository;

    [HttpGet]
    public async Task<List<Users>> Get() =>
        await _userRepository.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Users>> Get(string id)
    {
        var user = await _userRepository.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        return user;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Users newUser)
    {
        await _userRepository.CreateAsync(newUser);

        return CreatedAtAction(nameof(Get), new { id = newUser.Id }, newUser);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Users updatedUser)
    {
        var user = await _userRepository.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        updatedUser.Id = user.Id;

        await _userRepository.UpdateAsync(id, updatedUser);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _userRepository.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        await _userRepository.RemoveAsync(id);

        return NoContent();
    }
}