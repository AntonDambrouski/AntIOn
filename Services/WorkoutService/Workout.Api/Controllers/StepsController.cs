using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workout.Api.ApiModels.StepDTOs;
using Workout.Core.Extensions;
using Workout.Core.Interfaces.Repositories;
using Workout.Core.Models;

namespace Workout.Api.Controllers;

[Route("api/[controller]")]
[Authorize(Policy = "ApiScope")]
[ApiController]
public class StepsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uof;
    private readonly IValidator<Step> _validator;

    public StepsController(IMapper mapper, IUnitOfWork unitOfWork, IValidator<Step> validator)
    {
        _mapper = mapper;
        _uof = unitOfWork;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IEnumerable<StepDisplayDTO>> Get()
    {
        var steps = await _uof.StepRepository.GetAllAsync();
        var stepsDTOs = _mapper.Map<IEnumerable<StepDisplayDTO>>(steps);
        return stepsDTOs;
    }

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<StepDetailedDTO>> Get(string id)
    {
        var step = await _uof.StepRepository.GetByIdAsync(id);
        if (step is null)
        {
            return NotFound();
        }

        var stepDTO = _mapper.Map<StepDetailedDTO>(step);
        return stepDTO;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] StepCreateDTO item)
    {
        var step = _mapper.Map<Step>(item);
        var validatorResult = _validator.Validate(step);
        if (!validatorResult.IsValid)
        {
            return BadRequest(new
            {
                errorMessage = "The model input is invalid.",
                errors = validatorResult.GetValidatorErrors()
            });
        }

        await _uof.StepRepository.CreateAsync(step);
        return CreatedAtAction(nameof(Post), new { step.Id }, step);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromBody] StepUpdateDTO item)
    {
        if (id != item.Id)
        {
            return BadRequest();
        }

        var step = await _uof.StepRepository.GetByIdAsync(id);
        if (step is null)
        {
            return NotFound();
        }

        var updatedStep = _mapper.Map<Step>(step);
        var validatorResult = _validator.Validate(updatedStep);
        if (!validatorResult.IsValid)
        {
            return BadRequest(new
            {
                errorMessage = "The model input is invalid.",
                errors = validatorResult.GetValidatorErrors()
            });
        }

        updatedStep.Id = step.Id;
        await _uof.StepRepository.UpdateAsync(id, updatedStep);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var step = await _uof.StepRepository.GetByIdAsync(id);
        if (step is null)
        {
            return NotFound();
        }

        await _uof.StepRepository.DeleteAsync(id);
        return NoContent();
    }
}
