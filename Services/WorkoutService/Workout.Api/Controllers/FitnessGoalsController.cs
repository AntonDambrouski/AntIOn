using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workout.Api.ApiModels.ExerciseDTOs;
using Workout.Api.ApiModels.FitnessGoalDTOs;
using Workout.Api.ApiModels.UrlQueries;
using Workout.Api.Helpers;
using Workout.Api.Wrappers;
using Workout.Core.Interfaces.Services;
using Workout.Core.Models;

namespace Workout.Api.Controllers;

[Route("api/[controller]")]
[Authorize(Policy = "ApiScope")]
[ApiController]
public class FitnessGoalsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IFitnessGoalService _fitnessGoalService;
    private readonly IUrlService _urlService;

    public FitnessGoalsController(IMapper mapper, IFitnessGoalService fitnessGoalService, IUrlService urlService)
    {
        _mapper = mapper;
        _fitnessGoalService = fitnessGoalService;
        _urlService = urlService;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PaginationUrlQuery query)
    {
        var fitnessGoals = await _fitnessGoalService.GetPaginatedAsync(query.PageNumber, query.PageSize);
        var fitnessGoalDTOs = _mapper.Map<IEnumerable<FitnessGoalDisplayDTO>>(fitnessGoals);

        var route = Request.Path;
        var totalRecords = await _fitnessGoalService.GetRecordsCountAsync();

        var paginatedResponse = PaginationHelper.CreatePagedResponse(fitnessGoalDTOs, query, totalRecords, _urlService, route);

        return Ok(paginatedResponse);
    }

    [HttpGet("{id:length(24)}")]
    public async Task<IActionResult> Get(string id)
    {
        var fitnessGoal = await _fitnessGoalService.GetByIdAsync(id);
        if (fitnessGoal is null)
        {
            var error = new Error
            {
                Name = "Fitness goal isn't found",
                Message = $"Fitness goal with id: {id} doesn't exist"
            };

            var errorResponse = new Response<FitnessGoalDetailedDTO>(new[] { error }, "Not found");
            return NotFound(errorResponse);
        }

        var fitnessGoalDTO = _mapper.Map<FitnessGoalDetailedDTO>(fitnessGoal);
        var successfulResponse = new Response<FitnessGoalDetailedDTO>(fitnessGoalDTO);
        return Ok(successfulResponse);
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
