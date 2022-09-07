using FluentValidation.Results;
using Workout.Core.Models;

namespace Workout.Core.Extensions;

public static class Extensions
{
    public static IEnumerable<Error> GetValidatorErrors(this ValidationResult result)
        => result.Errors.Select(err => new Error
        {
            Message = err.ErrorMessage,
            Name = $"{err.PropertyName} is invalid"
        });
}
