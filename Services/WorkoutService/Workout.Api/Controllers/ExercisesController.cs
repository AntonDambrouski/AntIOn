using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workout.Api.ApiModels.ExerciseDTOs;
using Workout.Api.ApiModels.UrlQueries;
using Workout.Api.Helpers;
using Workout.Api.Wrappers;
using Workout.Core.Extensions;
using Workout.Core.Interfaces.Repositories;
using Workout.Core.Interfaces.Services;
using Workout.Core.Models;

namespace Workout.Api.Controllers;

[Route("api/[controller]")]
[Authorize(Policy = "ApiScope")]
[ApiController]
public class ExercisesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uof;
    private readonly IValidator<Exercise> _validator;
    private readonly IUrlService _urlService;

    public ExercisesController(
        IMapper mapper,
        IUnitOfWork unitOfWork, 
        IValidator<Exercise> validator, 
        IUrlService urlService)
    {
        _mapper = mapper;
        _uof = unitOfWork;
        _validator = validator;
        _urlService = urlService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get([FromQuery] PaginationUrlQuery query)
    {
        var exercises = await _uof.ExerciseRepository.GetPaginatedAsync(query.PageNumber, query.PageSize);
        var exerciseDTOs = _mapper.Map<IEnumerable<ExerciseDTO>>(exercises);
       
        var route = Request.Path;
        var totalCount = await _uof.ExerciseRepository.CountAsync();
        var pagedResponse = PaginationHelper.CreatePagedResponse(exerciseDTOs, query, totalCount, _urlService, route);
        
        return Ok(pagedResponse);
    }

    [HttpGet("{id:length(24)}")]
    public async Task<IActionResult> Get(string id)
    {
        var exercise = await _uof.ExerciseRepository.GetByIdAsync(id);
        if (exercise is null)
        {
            var error = new Error
            {
                Name = "Exercise isn't found",
                Message = $"Exercise with id: {id} doesn't exist"
            };

            var errorResponse = new Response<ExerciseDTO>(new[] { error }, "Not found");
            return NotFound(errorResponse);
        }

        var exerciseDTO = _mapper.Map<ExerciseDTO>(exercise);
        var successfulResponse = new Response<ExerciseDTO>(exerciseDTO);
        return Ok(successfulResponse);
    }

    [HttpPost]
    [AllowAnonymous]
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