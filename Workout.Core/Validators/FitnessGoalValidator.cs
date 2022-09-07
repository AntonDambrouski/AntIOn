using FluentValidation;
using Workout.Core.Models;

namespace Workout.Core.Validators;

public class FitnessGoalValidator : AbstractValidator<FitnessGoal>
{
	public FitnessGoalValidator()
	{
		RuleFor(fg => fg.Name).NotEmpty().Length(2, 30);
		RuleFor(fg => fg.Steps).NotNull().Must(steps => steps.Any());
		RuleFor(fg => fg.TargetSet).NotNull();
	}
}
