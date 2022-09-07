using System.ComponentModel.DataAnnotations;
using Workout.Core.Enums;

namespace Workout.Api.ApiModels.ExerciseDTOs;

public class ExerciseDTO
{
    public string? Id { get; set; }
    public string Name { get; set; }
}
