using MongoDB.Driver;
using Workout.Core.Interfaces.Repositories;
using Workout.Core.Models;

namespace Workout.Infrastructure.Data;

public class RepositoryBase<TDocument> : IRepository<TDocument>
    where TDocument : EntityBase
{
    protected readonly IMongoCollection<TDocument> _collection;

    protected RepositoryBase(IMongoCollection<TDocument> collection)
    {
        _collection = collection;
    }

    public async Task<long> CountAsync()
    {
        return await _collection.CountDocumentsAsync(_ => true);
    }

    public async Task CreateAsync(TDocument item)
    {
        await _collection.InsertOneAsync(item);
    }

    public async Task DeleteAsync(string id)
    {
        await _collection.DeleteOneAsync(item => item.Id == id);
    }

    public async Task<IEnumerable<TDocument>> GetAllAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    public async Task<TDocument?> GetByIdAsync(string id)
    {
        return await _collection.Find(item => item.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<TDocument>> GetPaginatedAsync(int pageNumber, int pageSize)
    {
        return await _collection
            .Find(_ => true)
            .Skip((pageNumber - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();
    }

    public async Task UpdateAsync(string id, TDocument item)
    {
        await _collection.ReplaceOneAsync(item => item.Id == id, item);
    }
}
