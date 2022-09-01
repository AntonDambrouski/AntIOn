using Workout.Core.Models;

namespace Workout.Api.ApiModels;

public class FitnessGoalDisplayDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsDone { get; set; }
}
