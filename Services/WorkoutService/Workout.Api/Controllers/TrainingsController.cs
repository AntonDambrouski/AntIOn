using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workout.Api.ApiModels.TrainingDTOs;
using Workout.Core.Interfaces.Services;
using Workout.Core.Models;

namespace Workout.Api.Controllers;

[Route("api/[controller]")]
[Authorize(Policy = "ApiScope")]
[ApiController]
public class TrainingsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ITrainingService _trainingService;

    public TrainingsController(IMapper mapper, ITrainingService trainingService)
    {
        _mapper = mapper;
        _trainingService = trainingService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IEnumerable<TrainingDisplayDTO>> Get()
    {
        var trainings = await _trainingService.GetAllAsync();
        var trainingDTOs = _mapper.Map<IEnumerable<TrainingDisplayDTO>>(trainings);
        return trainingDTOs;
    }

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<TrainingDetailedDTO>> Get(string id)
    {
        var training = await _trainingService.GetByIdAsync(id);
        if (training is null)
        {
            return NotFound();
        }

        var trainingDTO = _mapper.Map<TrainingDetailedDTO>(training);
        return trainingDTO;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] TrainingCreateDTO item)
    {
        var training = _mapper.Map<Training>(item);
        var creatingErrors = await _trainingService.CreateAsync(training, item.SetIds);
        if (creatingErrors is not null && creatingErrors.Any())
        {
            return BadRequest(new
            {
                errorMessage = "The model input is invalid.",
                errors = creatingErrors
            });
        }

        return CreatedAtAction(nameof(Post), new { training.Id }, training);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromBody] TrainingUpdateDTO item)
    {
        if (id != item.Id)
        {
            return BadRequest();
        }

        var training = await _trainingService.GetByIdAsync(id);
        if (training is null)
        {
            return NotFound();
        }

        var updatedTraining = _mapper.Map<Training>(item);
        updatedTraining.Id = training.Id;

        var updatingErrors = await _trainingService.UpdateAsync(updatedTraining, item.SetIds);
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
        var training = await _trainingService.GetByIdAsync(id);
        if (training is null)
        {
            return NotFound();
        }

        await _trainingService.DeleteAsync(id);
        return NoContent();
    }
}
