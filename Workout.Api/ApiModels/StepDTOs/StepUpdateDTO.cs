namespace Workout.Api.ApiModels.StepDTOs;

public class StepUpdateDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string IsCompleted { get; set; }
}
