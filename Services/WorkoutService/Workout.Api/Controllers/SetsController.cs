using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workout.Api.ApiModels.FitnessGoalDTOs;
using Workout.Api.ApiModels.SetDTOs;
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
public class SetsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ISetService _setService;
    private readonly IUrlService _urlService;

    public SetsController(IMapper mapper, ISetService setService, IUrlService urlService)
    {
        _mapper = mapper;
        _setService = setService;
        _urlService = urlService;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PaginationUrlQuery query)
    {
        var sets = await _setService.GetAllAsync();
        var setDTOs = _mapper.Map<IEnumerable<SetDisplayDTO>>(sets);

        var route = Request.Path;
        var totalRecords = await _setService.GetRecordsCountAsync();

        var paginatedResponse = PaginationHelper.CreatePagedResponse(setDTOs, query, totalRecords, _urlService, route);

        return Ok(paginatedResponse);
    }

    [HttpGet("{id:length(24)}")]
    public async Task<IActionResult> Get(string id)
    {
        var set = await _setService.GetByIdAsync(id);
        if (set is null)
        {
            var error = new Error
            {
                Name = "Set isn't found",
                Message = $"Set with id: {id} doesn't exist"
            };

            var errorResponse = new Response<SetDetailedDTO>(new[] { error }, "Not found");
            return NotFound(errorResponse);
        }

        var setDTO = _mapper.Map<SetDetailedDTO>(set);
        var successfulResponse = new Response<SetDetailedDTO>(setDTO);
        return Ok(successfulResponse);
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
