using Workout.Core.Models;

namespace Workout.Api.ApiModels.TrainingDTOs;

public class TrainingCreateDTO
{
    public string Name { get; set; }
    public IEnumerable<string> SetIds { get; set; }
}
