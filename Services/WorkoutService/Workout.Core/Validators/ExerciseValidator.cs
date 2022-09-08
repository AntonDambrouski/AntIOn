using FluentValidation;
using Workout.Core.Models;

namespace Workout.Core.Validators;

public class ExerciseValidator : AbstractValidator<Exercise>
{
	public ExerciseValidator()
	{
		RuleFor(ex => ex.Name).NotEmpty().Length(2, 20);
	}
}
