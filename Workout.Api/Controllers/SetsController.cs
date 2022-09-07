using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Workout.Api.ApiModels.SetDTOs;
using Workout.Core.Interfaces.Services;
using Workout.Core.Models;

namespace Workout.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SetsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ISetService _setService;

    public SetsController(IMapper mapper, ISetService setService)
    {
        _mapper = mapper;
        _setService = setService;
    }

    [HttpGet]
    public async Task<IEnumerable<SetDisplayDTO>> Get()
    {
        var sets = await _setService.GetAllAsync();
        var setDTOs = _mapper.Map<IEnumerable<SetDisplayDTO>>(sets);
        return setDTOs;
    }

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<SetDetailedDTO>> Get(string id)
    {
        var set = await _setService.GetByIdAsync(id);
        if (set is null)
        {
            return NotFound();
        }

        var setDTO = _mapper.Map<SetDetailedDTO>(set);
        return setDTO;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] SetCreateDTO item)
    {
        var set = _mapper.Map<Set>(item);
        var creatingErrors = await _setService.CreateAsync(set, item.ExerciseId);
        if (creatingErrors is not null && creatingErrors.Any())
        {
            return BadRequest(new
            {
                errorMessage = "The model input is invalid.",
                errors = creatingErrors
            });
        }

        return CreatedAtAction(nameof(Post), new { set.Id }, set);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromBody] SetUpdateDTO item)
    {
        if (id != item.Id)
        {
            return BadRequest();
        }

        var set = await _setService.GetByIdAsync(id);
        if (set is null)
        {
            return NotFound();
        }

        var updatedSet = _mapper.Map<Set>(item);
        var updatingErrors = await _setService.UpdateAsync(updatedSet, item.ExerciseId);
        if (updatingErrors is not null && updatingErrors.Any())
        {
            return BadRequest(new
            {
                errorMessage = "The model input is invalid.",
                errors = updatingErrors
            });
        }

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var set = await _setService.GetByIdAsync(id);
        if (set is null)
        {
            return NotFound();
        }

        await _setService.DeleteAsync(id);
        return NoContent();
    }
}
