using Workout.Core.Models;

namespace Workout.Api.ApiModels.TrainingDTOs;

public class TrainingCreateDTO
{
    public string Name { get; set; }
    public IEnumerable<Set> Sets { get; set; }
    public int Duration { get; set; }
}
