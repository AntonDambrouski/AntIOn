namespace Workout.Api.ApiModels.StepDTOs;

public class StepUpdateDTO
{
    public string? Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
}
