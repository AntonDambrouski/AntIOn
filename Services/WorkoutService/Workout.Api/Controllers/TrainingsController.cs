using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workout.Api.ApiModels.SetDTOs;
using Workout.Api.ApiModels.TrainingDTOs;
using Workout.Api.ApiModels.UrlQueries;
using Workout.Api.Helpers;
using Workout.Api.Wrappers;
using Workout.Core.Interfaces.Services;
using Workout.Core.Models;
using Workout.Core.Services;

namespace Workout.Api.Controllers;

[Route("api/[controller]")]
[Authorize(Policy = "ApiScope")]
[ApiController]
public class TrainingsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ITrainingService _trainingService;
    private readonly IUrlService _urlService;

    public TrainingsController(IMapper mapper, ITrainingService trainingService, IUrlService urlService)
    {
        _mapper = mapper;
        _trainingService = trainingService;
        _urlService = urlService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get([FromQuery] PaginationUrlQuery query)
    {
        var trainings = await _trainingService.GetAllAsync();
        var trainingDTOs = _mapper.Map<IEnumerable<TrainingDisplayDTO>>(trainings);

        var route = Request.Path;
        var totalRecords = await _trainingService.GetRecordsCountAsync();

        var paginatedResponse = PaginationHelper.CreatePagedResponse(trainingDTOs, query, totalRecords, _urlService, route);

        return Ok(paginatedResponse);
    }

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<TrainingDetailedDTO>> Get(string id)
    {
        var training = await _trainingService.GetByIdAsync(id);
        if (training is null)
        {
            var error = new Error
            {
                Name = "Training isn't found",
                Message = $"Training with id: {id} doesn't exist"
            };

            var errorResponse = new Response<TrainingDetailedDTO>(new[] { error }, "Not found");
            return NotFound(errorResponse);
        }

        var trainingDTO = _mapper.Map<TrainingDetailedDTO>(training);
        var successfulResponse = new Response<TrainingDetailedDTO>(trainingDTO);
        return Ok(successfulResponse);
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
