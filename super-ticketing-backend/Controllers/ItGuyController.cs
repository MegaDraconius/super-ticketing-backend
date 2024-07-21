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
    private readonly IMapper _mapper;

    public ItGuyController(IITGuyRepository itGuysRepository, IMapper mapper)
    {
        _itGuyRepository = itGuysRepository;
        _mapper = mapper;
    }


    [HttpGet]
    public async Task<List<ItGuyDto>> Get()
    {
        var itGuy = await _itGuyRepository.GetAsync();

        return _mapper.Map<List<ItGuyDto>>(itGuy);
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
        return itGuyDto;
    }

    [HttpPost]
    public async Task<IActionResult> Post(ItGuyCreateDto itGuyCreateDto)
    {
        var newItGuy = _mapper.Map<ITGuys>(itGuyCreateDto);
        await _itGuyRepository.CreateAsync(newItGuy);

        var itGuyDto = _mapper.Map<ItGuyDto>(newItGuy);
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