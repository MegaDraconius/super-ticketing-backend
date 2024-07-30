using System.Security.Cryptography;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.BearerToken;
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
    private readonly ICountryRepository _countryRepository;
    private readonly IMapper _mapper;

    public UserController(IUserRepository usersRepository, IMapper mapper, ICountryRepository countryRepository)
    {
        _userRepository = usersRepository;
        _mapper = mapper;
        _countryRepository = countryRepository;
    }


    [HttpGet]
    public async Task<List<UserDto>> Get()
    {
        var users = await _userRepository.GetAsync();
        var userDtos = _mapper.Map<List<UserDto>>(users);

        foreach (var userDto in userDtos)
        {
            var country = await _countryRepository.GetAsync(userDto.CountryId);
            userDto.CountryCode = country?.CountryCode;
        }

        return userDtos;
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
        var country = await _countryRepository.GetAsync(userDto.CountryId);
        userDto.CountryCode = country?.CountryCode;
        
        return userDto;
    }

    [HttpPost]
    public async Task<IActionResult> Post(UserCreateDto userCreateDto)
    {
        var country = await _countryRepository.GetByNameAsync(userCreateDto.CountryCode);
        if (country == null)
        {
            return BadRequest("Country not found");
        }

        var newUser = _mapper.Map<Users>(userCreateDto);
        newUser.CountryId = country.Id;

        await _userRepository.CreateAsync(newUser);

        var userDto = _mapper.Map<UserDto>(newUser);
        userDto.CountryCode = country.CountryCode;
        
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

        if (user.CountryId != updatedUserDto.CountryCode)
        {
            var country = await _countryRepository.GetByNameAsync(updatedUserDto.CountryCode);
            if (country == null)
            {
                return BadRequest("Country not found");
            }

            user.CountryId = country.Id;
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