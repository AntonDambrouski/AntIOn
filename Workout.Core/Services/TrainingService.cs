using FluentValidation;
using System.Collections;
using Workout.Core.Extensions;
using Workout.Core.Interfaces.Repositories;
using Workout.Core.Interfaces.Services;
using Workout.Core.Models;

namespace Workout.Core.Services;

public class TrainingService : ITrainingService
{
    private readonly IUnitOfWork _uof;
    private readonly IValidator<Training> _validator;

    public TrainingService(IUnitOfWork unitOfWork, IValidator<Training> validator)
    {
        _uof = unitOfWork;
        _validator = validator;
    }

    public async Task<IEnumerable<Error>?> CreateAsync(Training training, IEnumerable<string> setIds)
    {
        var assigningSetsErrors = await AssignSetsToTrainingAsync(training, setIds);
        if (assigningSetsErrors is not null && assigningSetsErrors.Any())
        {
            return assigningSetsErrors;
        }

        var validatorResult = _validator.Validate(training);
        if (!validatorResult.IsValid)
        {
            return validatorResult.GetValidatorErrors();
        }

        await _uof.TrainingRepository.CreateAsync(training);
        return null;
    }

    public async Task DeleteAsync(string id)
    {
        await _uof.TrainingRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<Training>> GetAllAsync()
    {
        return await _uof.TrainingRepository.GetAllAsync();
    }

    public async Task<Training?> GetByIdAsync(string id)
    {
        return await _uof.TrainingRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Error>?> UpdateAsync(Training training, IEnumerable<string> setIds)
    {
        var assigningSetsErrors = await AssignSetsToTrainingAsync(training, setIds);
        if (assigningSetsErrors is not null && assigningSetsErrors.Any())
        {
            return assigningSetsErrors;
        }

        var validatorResult = _validator.Validate(training);
        if (!validatorResult.IsValid)
        {
            return validatorResult.GetValidatorErrors();
        }

        await _uof.TrainingRepository.UpdateAsync(training.Id, training);
        return null;
    }

    private async Task<IEnumerable<Error>?> AssignSetsToTrainingAsync(Training training, IEnumerable<string> setIds)
    {
        var trainingSets = await _uof.SetRepository.GetByIdsAsync(setIds);
        if (trainingSets.Count() != setIds.Count())
        {
            return setIds
                 .Where(id => !trainingSets.Any(set => set.Id == id))
                 .Select(id => new Error
                 {
                     Name = "Set wasn't found",
                     Message = $"Set with id: {id} doesn't exist."
                 });
        }

        training.Sets = trainingSets;
        return null;
    }
}
