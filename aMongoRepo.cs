using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace aMongoLibrary;

public interface IMongoRepo<TDocument> where TDocument : IDocument
{
    IQueryable<TDocument> AsQueryable();

    IEnumerable<TDocument> FilterBy(
        Expression<Func<TDocument, bool>> filterExpression);

    IEnumerable<TProjected> FilterBy<TProjected>(
        Expression<Func<TDocument, bool>> filterExpression,
        Expression<Func<TDocument, TProjected>> projectionExpression);

    TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression);
    Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression);
    TDocument FindById(string id);
    Task<TDocument> FindByIdAsync(string id);
    void InsertOne(TDocument document);
    Task InsertOneAsync(TDocument document);
    void InsertMany(ICollection<TDocument> documents);
    Task InsertManyAsync(ICollection<TDocument> documents);
    Task ReplaceOneAsync(TDocument document);
    Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression);
    Task DeleteByIdAsync(string id);
    Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression);
}

public class MongoRepo<TDocument> //:IMongoRepo<TDocument> where TDocument : IDocument
{
    private readonly IMongoCollection<TDocument> _collection;

    public MongoRepo(IOptions<MongoSettings> mongoSettings)
    {
        IMongoClient client = new MongoClient(mongoSettings.Value.ConnectionURI);
        IMongoDatabase db = client.GetDatabase(mongoSettings.Value.DatabaseName);
        _collection = db.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));
    }

    private protected string GetCollectionName(Type documentType)
    {
        return ((BsonCollectionAttribute)documentType
                        .GetCustomAttributes(typeof(BsonCollectionAttribute),true)
                        .FirstOrDefault())?.CollectionName;
    }

    public virtual IQueryable<TDocument> AsQueryable()
    {
        return _collection.AsQueryable();
    }

    
    public async Task<IEnumerable<TDocument>> GetAllAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }
    
    

    public virtual IEnumerable<TDocument> FilterBy(
        Expression<Func<TDocument, bool>> filterExpression)
    {
        return _collection.Find(filterExpression).ToEnumerable();
    }

    public virtual IEnumerable<TProjected> FilterBy<TProjected>(
        Expression<Func<TDocument, bool>> filterExpression,
        Expression<Func<TDocument, TProjected>> projectionExpression)
    {
        return _collection.Find(filterExpression).Project(projectionExpression).ToEnumerable();
    }

    public virtual TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression)
    {
        return _collection.Find(filterExpression).FirstOrDefault();
    }

    public virtual Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        return Task.Run(() => _collection.Find(filterExpression).FirstOrDefaultAsync());
    }


    public virtual Task<TDocument> FindByIdAsync(string id)
    {
        return Task.Run(() =>
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TDocument>.Filter.Eq("Id", objectId);
            return _collection.Find(filter).SingleOrDefaultAsync();
        });
    }

    public virtual Task InsertOneAsync(TDocument document)
    {
        return Task.Run(() => _collection.InsertOneAsync(document));
    }

    public void InsertMany(ICollection<TDocument> documents)
    {
        _collection.InsertMany(documents);
    }

    public virtual async Task InsertManyAsync(ICollection<TDocument> documents)
    {
        await _collection.InsertManyAsync(documents);
    }

    public virtual async Task ReplaceOneAsync(TDocument document)
    {
        var filter = Builders<TDocument>.Filter.Eq("Id", document);
        await _collection.FindOneAndReplaceAsync(filter, document);
    }

    ///

    public async Task UpdateRecordAsync(string id, TDocument document)
    {
        var objectId = new ObjectId(id);
        var filter = Builders<TDocument>.Filter.Eq("Id", objectId);
        await _collection.FindOneAndReplaceAsync(filter, document);
    }

    //


    public Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        return Task.Run(() => _collection.FindOneAndDeleteAsync(filterExpression));
    }

    public Task DeleteByIdAsync(string id)
    {
        return Task.Run(() =>
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TDocument>.Filter.Eq("Id", objectId);
            _collection.FindOneAndDeleteAsync(filter);
        });
    }

    public Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        return Task.Run(() => _collection.DeleteManyAsync(filterExpression));
    }

}