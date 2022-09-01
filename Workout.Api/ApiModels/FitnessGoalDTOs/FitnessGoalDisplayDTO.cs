using Workout.Core.Models;

namespace Workout.Api.ApiModels.FitnessGoalDTOs;

public class FitnessGoalDisplayDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsDone { get; set; }
}
