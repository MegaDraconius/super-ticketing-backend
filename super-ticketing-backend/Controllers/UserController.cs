using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using super_ticketing_backend.Dto_s;
using super_ticketing_backend.Models;
using super_ticketing_backend.Repositories;


namespace super_ticketing_backend.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UserController : ControllerBase
{
    
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserController(IUserRepository usersRepository, IMapper mapper)
    {
        _userRepository = usersRepository;
        _mapper = mapper;
    }
        

    [HttpGet]
    public async Task<List<UserDto>> Get()
    {
        var user = await _userRepository.GetAsync();

        return _mapper.Map<List<UserDto>>(user);
    }
       

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<UserDto>> Get(string id)
    {
        var user = await _userRepository.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        var userDto = _mapper.Map<UserDto>(user);
        return userDto;
    }

    [HttpPost]
    public async Task<IActionResult> Post(UserCreateDto userCreateDto)
    {
        var newUser = _mapper.Map<Users>(userCreateDto);
        await _userRepository.CreateAsync(newUser);

        var userDto = _mapper.Map<UserDto>(newUser);
        return CreatedAtAction(nameof(Get), new { id = userDto.Id }, userDto);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, UserCreateDto updatedUserDto)
    {
        var user = await _userRepository.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        _mapper.Map(updatedUserDto, user);
        await _userRepository.UpdateAsync(user);
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