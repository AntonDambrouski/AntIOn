using Workout.Core.Models;

namespace Workout.Api.ApiModels.FitnessGoalDTOs;

public class FitnessGoalUpdateDTO
{
    public string? Id { get; set; }
    public string Name { get; set; }
    public Set TargetSet { get; set; }
    public IEnumerable<Step> Steps { get; set; }
    public bool IsDone { get; set; }
}
