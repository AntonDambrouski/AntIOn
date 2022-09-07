using Workout.Core.Models;

namespace Workout.Api.ApiModels.FitnessGoalDTOs;

public class FitnessGoalCreateDTO
{
    public string Name { get; set; }
    public string TargetSetId { get; set; }
    public IEnumerable<string> StepIds { get; set; }
}
