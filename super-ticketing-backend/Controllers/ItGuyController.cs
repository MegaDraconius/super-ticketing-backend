using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using super_ticketing_backend.Dto_s;
using super_ticketing_backend.Models;
using super_ticketing_backend.Repositories;

namespace super_ticketing_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItGuyController : ControllerBase
{
    private readonly IITGuyRepository _itGuyRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly IMapper _mapper;

    public ItGuyController(IITGuyRepository itGuysRepository, ICountryRepository countryRepository, IMapper mapper)
    {
        _itGuyRepository = itGuysRepository;
        _countryRepository = countryRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<List<ItGuyDto>> Get()
    {
        var itGuys = await _itGuyRepository.GetAsync();
        var itGuyDtos = _mapper.Map<List<ItGuyDto>>(itGuys);

        foreach (var itGuyDto in itGuyDtos)
        {
            var country = await _countryRepository.GetAsync(itGuyDto.CountryId);
            itGuyDto.CountryCode = country?.CountryCode;
        }

        return itGuyDtos;
    }

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<ItGuyDto>> Get(string id)
    {
        var itGuy = await _itGuyRepository.GetAsync(id);

        if (itGuy is null)
        {
            return NotFound();
        }

        var itGuyDto = _mapper.Map<ItGuyDto>(itGuy);
        var country = await _countryRepository.GetAsync(itGuy.CountryId);
        itGuyDto.CountryCode = country?.CountryCode;

        return itGuyDto;
    }

    [HttpPost]
    public async Task<IActionResult> Post(ItGuyCreateDto itGuyCreateDto)
    {
        // Buscar el país por nombre
        var country = await _countryRepository.GetByNameAsync(itGuyCreateDto.CountryCode);
        if (country == null)
        {
            return BadRequest("Country not found.");
        }

        var newItGuy = _mapper.Map<ITGuys>(itGuyCreateDto);
        newItGuy.CountryId = country.Id; // Asignar el CountryId basado en el Country encontrado

        await _itGuyRepository.CreateAsync(newItGuy);

        var itGuyDto = _mapper.Map<ItGuyDto>(newItGuy);
        itGuyDto.CountryCode = country.CountryCode;

        return CreatedAtAction(nameof(Get), new { id = itGuyDto.Id }, itGuyDto);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, ItGuyCreateDto updatedItGuyDto)
    {
        var itGuy = await _itGuyRepository.GetAsync(id);

        if (itGuy is null)
        {
            return NotFound();
        }

        // Buscar el país por el nuevo CountryCode si ha cambiado
        if (itGuy.CountryId != updatedItGuyDto.CountryCode)
        {
            var country = await _countryRepository.GetByNameAsync(updatedItGuyDto.CountryCode);
            if (country == null)
            {
                return BadRequest("Country not found.");
            }

            itGuy.CountryId = country.Id; // Actualizar el CountryId basado en el Country encontrado
        }

        // Mapear los cambios de ItGuyCreateDto al ITGuys existente
        _mapper.Map(updatedItGuyDto, itGuy);

        await _itGuyRepository.UpdateAsync(itGuy);

        return NoContent();
    }



    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var itGuy = await _itGuyRepository.GetAsync(id);

        if (itGuy is null)
        {
            return NotFound();
        }

        await _itGuyRepository.RemoveAsync(id);

        return NoContent();
    }
}

