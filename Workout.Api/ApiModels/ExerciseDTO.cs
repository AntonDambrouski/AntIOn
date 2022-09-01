using Workout.Core.Enums;

namespace Workout.Api.ApiModels;

public class ExerciseDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ExerciseType Type { get; set; }
}
