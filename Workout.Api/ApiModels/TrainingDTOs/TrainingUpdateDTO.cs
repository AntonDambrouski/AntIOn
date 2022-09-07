using Workout.Core.Models;

namespace Workout.Api.ApiModels.TrainingDTOs;

public class TrainingUpdateDTO
{
    public string? Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<string> SetIds { get; set; }
}
