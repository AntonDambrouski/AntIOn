using FluentValidation;
using Workout.Core.Extensions;
using Workout.Core.Interfaces.Repositories;
using Workout.Core.Interfaces.Services;
using Workout.Core.Models;

namespace Workout.Core.Services;

public class FitnessGoalService : IFitnessGoalService
{
    private readonly IUnitOfWork _uof;
    private readonly IValidator<FitnessGoal> _validator;

    public FitnessGoalService(IUnitOfWork unitOfWork, IValidator<FitnessGoal> validator)
    {
        _uof = unitOfWork;
        _validator = validator;
    }

    public async Task<IEnumerable<Error>?> CreateAsync(FitnessGoal fitnessGoal, IEnumerable<string> stepIds, string setId)
    {
        var assigningStepsErrors = await AssignStepsToFitnessGoalAsync(fitnessGoal, stepIds);
        if (assigningStepsErrors is not null && assigningStepsErrors.Any())
        {
            return assigningStepsErrors;
        }

        var assigningSetErrors = await AssignSetToFitnessGoalAsync(fitnessGoal, setId);
        if (assigningSetErrors is not null && assigningSetErrors.Any())
        {
            return assigningSetErrors;
        }

        var validatorResult = _validator.Validate(fitnessGoal);
        if (!validatorResult.IsValid)
        {
            return validatorResult.GetValidatorErrors();
        }

        await _uof.FitnessGoalRepository.CreateAsync(fitnessGoal);
        return null;
    }

    public async Task DeleteAsync(string id)
    {
        await _uof.FitnessGoalRepository.DeleteAsync(id);
    }

    public Task<IEnumerable<FitnessGoal>> GetAllAsync()
    {
        return _uof.FitnessGoalRepository.GetAllAsync();
    }

    public async Task<FitnessGoal?> GetByIdAsync(string id)
    {
        return await _uof.FitnessGoalRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Error>?> UpdateAsync(FitnessGoal fitnessGoal, IEnumerable<string> stepIds, string setId)
    {
        var assigningStepsErrors = await AssignStepsToFitnessGoalAsync(fitnessGoal, stepIds);
        if (assigningStepsErrors is not null && assigningStepsErrors.Any())
        {
            return assigningStepsErrors;
        }

        var assigningSetErrors = await AssignSetToFitnessGoalAsync(fitnessGoal, setId);
        if (assigningSetErrors is not null && assigningSetErrors.Any())
        {
            return assigningSetErrors;
        }

        var validatorResult = _validator.Validate(fitnessGoal);
        if (!validatorResult.IsValid)
        {
            return validatorResult.GetValidatorErrors();
        }

        await _uof.FitnessGoalRepository.UpdateAsync(fitnessGoal.Id, fitnessGoal);
        return null;
    }

    private async Task<IEnumerable<Error>?> AssignStepsToFitnessGoalAsync(FitnessGoal fitnessGoal, IEnumerable<string> stepIds)
    {
        var distinctStepIds = stepIds.Distinct().ToList();
        var goalSteps = await _uof.StepRepository.GetByIdsAsync(distinctStepIds);
        if (goalSteps.Count() != distinctStepIds.Count)
        {
            return distinctStepIds
                .Where(id => !goalSteps.Any(step => step.Id == id))
                .Select(id => new Error
                {
                    Name = "Step wasn't found",
                    Message = $"Step with id: {id} doesn't exist."
                });
        }

        fitnessGoal.Steps = goalSteps;
        return null;
    }

    private async Task<IEnumerable<Error>?> AssignSetToFitnessGoalAsync(FitnessGoal fitnessGoal, string setId)
    {
        var set = await _uof.SetRepository.GetByIdAsync(setId);
        if (set is null)
        {
            var error = new Error
            {
                Name = "Set wasn't found",
                Message = $"Set with id: {setId} doesn't exist."
            };
            return new[] { error };
        }

        fitnessGoal.TargetSet = set;
        return null;
    }
}
