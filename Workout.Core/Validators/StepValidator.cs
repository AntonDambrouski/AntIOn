using FluentValidation;
using Workout.Core.Models;

namespace Workout.Core.Validators;

public class StepValidator : AbstractValidator<Step>
{
	public StepValidator()
	{
		RuleFor(s => s.Name).NotEmpty().Length(2, 30);
	}
}
