using Workout.Core.Models;

namespace Workout.Api.ApiModels.TrainingDTOs;

public class TrainingDetailedDTO
{
    public string? Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<Set> Sets { get; set; }
    public int Duration { get; set; }
}
