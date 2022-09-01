using Workout.Core.Models;

namespace Workout.Api.ApiModels.FitnessGoalDTOs;

public class FitnessGoalCreateDTO
{
    public string Name { get; set; }
    public Set TargetSet { get; set; }
    public IEnumerable<Step> Steps { get; set; }
}
