using FluentValidation;
using Workout.Core.Constants;
using Workout.Core.Models;

namespace Workout.Core.Validators;

public class SetValidator : AbstractValidator<Set>
{
	public SetValidator()
	{
		RuleFor(s => s.Rest).InclusiveBetween(1, WorkoutManifest.MaxRestInSeconds)
			.WithMessage("The rest can't be less than 1 second and more than 60 minutes.");
		RuleFor(s => s.Exercise).NotNull();
		RuleFor(s => s.ValueUnit).IsInEnum();
		RuleFor(s => s.Value).Must(val => val is null || val.Value > 0 && val.Value < WorkoutManifest.MaxOfSetValue);
	}
}
