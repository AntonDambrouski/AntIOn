using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Workout.Core.Models;

public class FitnessGoal
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Name { get; set; }
    public Set TargetSet { get; set; }
    public IEnumerable<Step> Steps { get; set; }
    public bool IsDone => Steps.All(step => step.IsCompleted);
}
