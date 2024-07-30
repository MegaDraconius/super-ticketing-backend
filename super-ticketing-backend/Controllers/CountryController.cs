using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using super_ticketing_backend.Dto_s;
using super_ticketing_backend.Models;
using super_ticketing_backend.Repositories;

namespace super_ticketing_backend.Controllers;

[ApiController]
[Route("api/[controller]")]

public class CountryController : ControllerBase
{
    private readonly ICountryRepository _countryRepository;
    private readonly IMapper _mapper;

    public CountryController(ICountryRepository countryRepository, IMapper mapper)
    {
        _countryRepository = countryRepository;
        _mapper = mapper;
    }

    [HttpGet]

    public async Task<List<CountryDto>> Get()
    {
        var countries = await _countryRepository.GetAsync();

        return _mapper.Map<List<CountryDto>>(countries);
    }

    [HttpGet("{id:length(24)}")]

    public async Task<ActionResult<CountryDto>> Get(string id)
    {
        var country = await _countryRepository.GetAsync(id);

        if (country is null)
        {
            return NotFound();
        }

        var countryDto = _mapper.Map<CountryDto>(country);
        return countryDto;
    }

    [HttpPost]
    public async Task<IActionResult> Post(CountryCreateDto ticketCreateDto)
    {
        var newCountry = _mapper.Map<Country>(ticketCreateDto);
        await _countryRepository.CreateAsync(newCountry);

        var countryDto = _mapper.Map<CountryDto>(newCountry);
        return CreatedAtAction(nameof(Get), new { id = countryDto.Id }, countryDto);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, CountryCreateDto updatedCountryDto)
    {
        var country = await _countryRepository.GetAsync(id);

        if (country is null)
        {
            return NotFound();
        }

        _mapper.Map(updatedCountryDto, country);
        await _countryRepository.UpdateAsync(country);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var country = await _countryRepository.GetAsync(id);
        
            if (country is null)
            {
                return NotFound();
            }

            await _countryRepository.RemoveAsync(id);

            return NoContent();
        
    }
}
