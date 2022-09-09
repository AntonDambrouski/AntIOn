using FluentValidation;
using Workout.Core.Extensions;
using Workout.Core.Interfaces.Repositories;
using Workout.Core.Interfaces.Services;
using Workout.Core.Models;

namespace Workout.Core.Services;

public class SetService : ISetService
{
    private readonly IUnitOfWork _uof;
    private readonly IValidator<Set> _validator;

    public SetService(IUnitOfWork unitOfWork, IValidator<Set> validator)
    {
        _uof = unitOfWork;
        _validator = validator;
    }

    public async Task<IEnumerable<Error>?> CreateAsync(Set set, string exerciseId)
    {
        var assigningExerciseErrors = await AssignExerciseToSetAsync(set, exerciseId);
        if (assigningExerciseErrors is not null && assigningExerciseErrors.Any())
        {
            return assigningExerciseErrors;
        }

        var validatorResult = _validator.Validate(set);
        if (!validatorResult.IsValid)
        {
            return validatorResult.GetValidatorErrors();
        }

        await _uof.SetRepository.CreateAsync(set);
        return null;
    }

    public async Task DeleteAsync(string id)
    {
        await _uof.SetRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<Set>> GetAllAsync()
    {
        return await _uof.SetRepository.GetAllAsync();
    }

    public async Task<Set?> GetByIdAsync(string id)
    {
        return await _uof.SetRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Set>> GetPaginatedAsync(int pageNumber, int pageSize)
    {
        return await _uof.SetRepository.GetPaginatedAsync(pageNumber, pageSize);
    }

    public async Task<long> GetRecordsCountAsync()
    {
        return await _uof.SetRepository.CountAsync();
    }

    public async Task<IEnumerable<Error>?> UpdateAsync(Set set, string exerciseId)
    {
        var assigningExerciseErrors = await AssignExerciseToSetAsync(set, exerciseId);
        if (assigningExerciseErrors is not null && assigningExerciseErrors.Any())
        {
            return assigningExerciseErrors;
        }

        var validatorResult = _validator.Validate(set);
        if (!validatorResult.IsValid)
        {
            return validatorResult.GetValidatorErrors();
        }

        await _uof.SetRepository.UpdateAsync(set.Id, set);
        return null;
    }

    private async Task<IEnumerable<Error>?> AssignExerciseToSetAsync(Set set, string exerciseId)
    {
        var exercise = await _uof.ExerciseRepository.GetByIdAsync(exerciseId);
        if (exercise is not null)
        {
            set.Exercise = exercise;
            return null;
        }

        var error = new Error
        {
            Name = "Exercise wasn't found",
            Message = $"Exercise with id: {exerciseId} doesn't exist."
        };

        return new[] { error };
    }
}
