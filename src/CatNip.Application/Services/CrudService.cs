using CatNip.Domain.Models.Interfaces;
using CatNip.Domain.Repositories;
using CatNip.Domain.Services;

namespace CatNip.Application.Services;

public abstract class CrudService<TRepository, TModel, TId> : ICrudService<TModel, TId>
    where TRepository : ICrudRepository<TModel, TId>
    where TModel : IModel<TId>
    where TId : IEquatable<TId>
{
    protected CrudService(TRepository repository)
    {
        Repository = repository;
    }

    protected virtual TRepository Repository { get; init; }

    public virtual async Task<IEnumerable<TModel>> GetAllAsync(CancellationToken cancellation = default)
    {
        var result = await Repository.GetAllAsync(cancellation);

        return result;
    }

    public virtual async Task<int> CountAsync(CancellationToken cancellation = default)
    {
        int count = await Repository.CountAsync(cancellation);

        return count;
    }

    public virtual async Task<TModel> GetByIdAsync(TId id, CancellationToken cancellation = default)
    {
        var result = await Repository.GetByIdAsync(id, cancellation);

        return result;
    }

    public virtual async Task<bool> ExistsAsync(TId id, CancellationToken cancellation = default)
    {
        bool exists = await Repository.ExistsAsync(id, cancellation);

        return exists;
    }

    public virtual async Task<TModel> CreateAsync(TModel model, CancellationToken cancellation = default)
    {
        var result = await Repository.CreateAsync(model, cancellation);

        return result;
    }

    public virtual async Task UpdateAsync(TModel model, CancellationToken cancellation = default)
    {
        await Repository.UpdateAsync(model, cancellation);
    }

    public virtual async Task UpdateAsync(TId id, TModel model, CancellationToken cancellation = default)
    {
        await Repository.UpdateAsync(id, model, cancellation);
    }

    public virtual async Task DeleteAsync(TModel model, CancellationToken cancellation = default)
    {
        await Repository.DeleteAsync(model, cancellation);
    }

    public virtual async Task DeleteAsync(TId id, CancellationToken cancellation = default)
    {
        await Repository.DeleteAsync(id, cancellation);
    }
}
