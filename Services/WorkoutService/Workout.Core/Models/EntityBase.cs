using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Workout.Core.Models;

public abstract class EntityBase
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
}
