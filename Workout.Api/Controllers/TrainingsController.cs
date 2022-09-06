using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Workout.Api.ApiModels.TrainingDTOs;
using Workout.Core.Interfaces.Repositories;
using Workout.Core.Models;

namespace Workout.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TrainingsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uof;

    public TrainingsController(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _uof = unitOfWork;
    }

    [HttpGet]
    public async Task<IEnumerable<TrainingDisplayDTO>> Get()
    {
        var trainings = await _uof.TrainingRepository.GetAllAsync();
        var trainingDTOs = _mapper.Map<IEnumerable<TrainingDisplayDTO>>(trainings);
        return trainingDTOs;
    }

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<TrainingDetailedDTO>> Get(string id)
    {
        var training = await _uof.TrainingRepository.GetByIdAsync(id);
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
        await _uof.TrainingRepository.CreateAsync(training);
        return CreatedAtAction(nameof(Post), new { training.Id }, training);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromBody] TrainingUpdateDTO item)
    {
        if (id != item.Id)
        {
            return BadRequest();
        }

        var training = await _uof.TrainingRepository.GetByIdAsync(id);
        if (training is null)
        {
            return NotFound();
        }

        training.Name = item.Name;
        training.Sets = item.Sets;

        await _uof.TrainingRepository.UpdateAsync(id, training);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var training = await _uof.TrainingRepository.GetByIdAsync(id);
        if (training is null)
        {
            return NotFound();
        }

        await _uof.TrainingRepository.DeleteAsync(id);
        return NoContent();
    }
}
