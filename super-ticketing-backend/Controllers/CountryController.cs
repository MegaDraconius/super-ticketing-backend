using Microsoft.AspNetCore.Mvc;
using super_ticketing_backend.Models;
using super_ticketing_backend.Repositories;

namespace super_ticketing_backend.Controllers;

[ApiController]
[Route("api/[controller]")]

public class CountryController : ControllerBase
{
    private readonly ICountryRepository _countryRepository;

    public CountryController(ICountryRepository countryRepository) =>
        _countryRepository = countryRepository;

    [HttpGet]

    public async Task<List<Country>> Get() =>
        await _countryRepository.GetAsync();

    [HttpGet("{id:length(24)}")]

    public async Task<ActionResult<Country>> Get(string id)
    {
        var country = await _countryRepository.GetAsync(id);

        if (country is null)
        {
            return NotFound();
        }

        return country;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Country newCountry)
    {
        await _countryRepository.CreateAsync(newCountry);
        
        return CreatedAtAction(nameof(Get), new { id = newCountry.Id }, newCountry);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Country updateCountry)
    {
        var country = await _countryRepository.GetAsync(id);

        if (country is null)
        {
            return NotFound();
        }

        updateCountry.Id = country.Id;

        await _countryRepository.UpdateAsync(id, updateCountry);

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
