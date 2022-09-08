using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Workout.Api.ApiModels.FitnessGoalDTOs;
using Workout.Core.Interfaces.Services;
using Workout.Core.Models;

namespace Workout.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FitnessGoalsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IFitnessGoalService _fitnessGoalService;

    public FitnessGoalsController(IMapper mapper, IFitnessGoalService fitnessGoalService)
    {
        _mapper = mapper;
        _fitnessGoalService = fitnessGoalService;
    }

    [HttpGet]
    public async Task<IEnumerable<FitnessGoalDisplayDTO>> Get()
    {
        var fitnessGoals = await _fitnessGoalService.GetAllAsync();
        var fitnessGoalDTOs = _mapper.Map<IEnumerable<FitnessGoalDisplayDTO>>(fitnessGoals);
        return fitnessGoalDTOs;
    }

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<FitnessGoalDetailedDTO>> Get(string id)
    {
        var fitnessGoal = await _fitnessGoalService.GetByIdAsync(id);
        if (fitnessGoal is null)
        {
            return NotFound();
        }

        var fitnessGoalDTO = _mapper.Map<FitnessGoalDetailedDTO>(fitnessGoal);
        return fitnessGoalDTO;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] FitnessGoalCreateDTO item)
    {
        var fitnessGoal = _mapper.Map<FitnessGoal>(item);
        var creatingErrors = await _fitnessGoalService.CreateAsync(fitnessGoal, item.StepIds, item.TargetSetId);
        if (creatingErrors is not null && creatingErrors.Any())
        {
            return BadRequest(new
            {
                errorMessage = "The model input is invalid.",
                errors = creatingErrors
            });
        }

        return CreatedAtAction(nameof(Post), new { fitnessGoal.Id }, fitnessGoal);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromBody] FitnessGoalUpdateDTO item)
    {
        if (id != item.Id)
        {
            return BadRequest();
        }

        var fitnessGoal = await _fitnessGoalService.GetByIdAsync(id);
        if (fitnessGoal is null)
        {
            return NotFound();
        }

        var updatedFitnessGoal = _mapper.Map<FitnessGoal>(item);
        updatedFitnessGoal.Id = fitnessGoal.Id;
        
        var updatingErrors = await _fitnessGoalService.UpdateAsync(updatedFitnessGoal, item.StepIds, item.TargetSetId);
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
        var fitnessGoal = await _fitnessGoalService.GetByIdAsync(id);
        if (fitnessGoal is null)
        {
            return NotFound();
        }

        await _fitnessGoalService.DeleteAsync(id);
        return NoContent();
    }
}
