using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace aMongoLibrary;

public class MongoSettings
{
    public string ConnectionURI { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
}

public interface IDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    ObjectId Id { get; set; }
    
}

public abstract class Document : IDocument
{
    public ObjectId Id { get; set; }
    
}

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class BsonCollectionAttribute : Attribute
{
    public string CollectionName { get; }

    public BsonCollectionAttribute(string collectionName)
    {
        CollectionName = collectionName;
    }
}
