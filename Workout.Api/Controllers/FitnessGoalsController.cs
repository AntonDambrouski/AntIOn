using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Workout.Api.ApiModels.FitnessGoalDTOs;
using Workout.Core.Interfaces.Repositories;
using Workout.Core.Models;

namespace Workout.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FitnessGoalsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uof;

    public FitnessGoalsController(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _uof = unitOfWork;
    }

    [HttpGet]
    public async Task<IEnumerable<FitnessGoalDisplayDTO>> Get()
    {
        var fitnessGoals = await _uof.FitnessGoalRepository.GetAllAsync();
        var fitnessGoalDTOs = _mapper.Map<IEnumerable<FitnessGoalDisplayDTO>>(fitnessGoals);
        return fitnessGoalDTOs;
    }

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<FitnessGoalDetailedDTO>> Get(string id)
    {
        var fitnessGoal = await _uof.FitnessGoalRepository.GetByIdAsync(id);
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
        await _uof.FitnessGoalRepository.CreateAsync(fitnessGoal);
        return CreatedAtAction(nameof(Post), new { fitnessGoal.Id }, fitnessGoal);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromBody] FitnessGoalUpdateDTO item)
    {
        if (id != item.Id)
        {
            return BadRequest();
        }

        var fitnessGoal = await _uof.FitnessGoalRepository.GetByIdAsync(id);
        if (fitnessGoal is null)
        {
            return NotFound();
        }

        fitnessGoal.Name = item.Name;
        fitnessGoal.Steps = item.Steps;
        fitnessGoal.TargetSet = item.TargetSet; 

        await _uof.FitnessGoalRepository.UpdateAsync(id, fitnessGoal);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var fitnessGoal = await _uof.FitnessGoalRepository.GetByIdAsync(id);
        if (fitnessGoal is null)
        {
            return NotFound();
        }

        await _uof.FitnessGoalRepository.DeleteAsync(id);
        return NoContent();
    }
}
