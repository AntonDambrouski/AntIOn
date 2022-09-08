using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Workout.Core.Models;

public class Step
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Name { get; set; }
    public bool IsCompleted { get; set; }
}
