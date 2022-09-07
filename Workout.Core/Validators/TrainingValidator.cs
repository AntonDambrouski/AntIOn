using FluentValidation;
using Workout.Core.Models;

namespace Workout.Core.Validators;

public class TrainingValidator : AbstractValidator<Training>
{
	public TrainingValidator()
	{
		RuleFor(t => t.Name).NotEmpty().Length(2, 30);
		RuleFor(t => t.Sets).NotNull().Must(sets => sets.Any());
		RuleFor(t => t.Duration).GreaterThan(0);
	}
}
