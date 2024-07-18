using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using super_ticketing_backend.Models;
using super_ticketing_backend.Repositories;

namespace super_ticketing_backend.Controllers;

[ApiController]
[Route("api/[controller]")]

public class ItGuyController : ControllerBase
{
    private readonly IITGuyRepository _itGuyRepository;

    public ItGuyController(IITGuyRepository itGuysRepository) =>
        _itGuyRepository = itGuysRepository;

    [HttpGet]
    public async Task<List<ITGuys>> Get() =>
        await _itGuyRepository.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<ITGuys>> Get(string id)
    {
        var user = await _itGuyRepository.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        return user;
    }

    [HttpPost]
    public async Task<IActionResult> Post(ITGuys newItGuy)
    {
        await _itGuyRepository.CreateAsync(newItGuy);

        return CreatedAtAction(nameof(Get), new { id = newItGuy.Id }, newItGuy);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, ITGuys updatediItGuy)
    {
        var user = await _itGuyRepository.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        updatediItGuy.Id = user.Id;

        await _itGuyRepository.UpdateAsync(id, updatediItGuy);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _itGuyRepository.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        await _itGuyRepository.RemoveAsync(id);

        return NoContent();
    }
}