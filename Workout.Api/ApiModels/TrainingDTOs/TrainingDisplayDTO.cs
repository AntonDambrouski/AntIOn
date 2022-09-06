using Workout.Core.Models;

namespace Workout.Api.ApiModels.TrainingDTOs;

public class TrainingDisplayDTO
{
    public string? Id { get; set; }
    public string Name { get; set; }
    public int Duration { get; set; }
}
