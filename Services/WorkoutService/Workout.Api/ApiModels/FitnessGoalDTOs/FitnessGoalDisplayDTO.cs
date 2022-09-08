using Workout.Core.Models;

namespace Workout.Api.ApiModels.FitnessGoalDTOs;

public class FitnessGoalDisplayDTO
{
    public string? Id { get; set; }
    public string Name { get; set; }
    public bool IsDone { get; set; }
}
