using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Workout.Api.ApiModels.ExerciseDTOs;
using Workout.Core.Extensions;
using Workout.Core.Interfaces.Repositories;
using Workout.Core.Models;

namespace Workout.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExercisesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uof;
    private readonly IValidator<Exercise> _validator;

    public ExercisesController(IMapper mapper, IUnitOfWork unitOfWork, IValidator<Exercise> validator)
    {
        _mapper = mapper;
        _uof = unitOfWork;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IEnumerable<ExerciseDTO>> Get()
    {
        var exercises = await _uof.ExerciseRepository.GetAllAsync();
        var exerciseDTOs = _mapper.Map<IEnumerable<ExerciseDTO>>(exercises);
        return exerciseDTOs;
    }

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<ExerciseDTO>> Get(string id)
    {
        var exercise = await _uof.ExerciseRepository.GetByIdAsync(id);
        if (exercise is null)
        {
            return NotFound();
        }

        var exerciseDTO = _mapper.Map<ExerciseDTO>(exercise);
        return exerciseDTO;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ExerciseCreateDTO item)
    {
        var exercise = _mapper.Map<Exercise>(item);
        var validatorResult = _validator.Validate(exercise);
        if (!validatorResult.IsValid)
        {
            return BadRequest(new
            {
                errorMessage = "The model input is invalid.",
                errors = validatorResult.GetValidatorErrors()
            });
        }

        await _uof.ExerciseRepository.CreateAsync(exercise);
        return CreatedAtAction(nameof(Post), new { exercise.Id }, exercise);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromBody] ExerciseDTO item)
    {
        if (id != item.Id)
        {
            return BadRequest();
        }

        var exercise = await _uof.ExerciseRepository.GetByIdAsync(id);
        if (exercise is null)
        {
            return NotFound();
        }

        var updatedExercise = _mapper.Map<Exercise>(item);
        var validatorResult = _validator.Validate(updatedExercise);
        if (!validatorResult.IsValid)
        {
            return BadRequest(new
            {
                errorMessage = "The model input is invalid.",
                errors = validatorResult.GetValidatorErrors()
            });
        }

        updatedExercise.Id = exercise.Id;
        await _uof.ExerciseRepository.UpdateAsync(id, updatedExercise);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var exercise = await _uof.ExerciseRepository.GetByIdAsync(id);
        if (exercise is null)
        {
            return NotFound();
        }

        await _uof.ExerciseRepository.DeleteAsync(id);
        return NoContent();
    }
}