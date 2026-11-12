using CatNip.Domain.Models.Interfaces;
using CatNip.Domain.Repositories;
using CatNip.Domain.Services;

namespace CatNip.Application.Services;

public abstract class CrudService<TRepository, TModel, TId> : Service<TRepository, TModel>, ICrudService<TModel, TId>
    where TRepository : ICrudRepository<TModel, TId>
    where TModel : ICrudModel<TId>
    where TId : IEquatable<TId>
{
    protected CrudService(TRepository repository)
        : base(repository)
    {
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

    public virtual async Task UpdateAsync(TId id, TModel model, CancellationToken cancellation = default)
    {
        await Repository.UpdateAsync(id, model, cancellation);
    }

    public virtual async Task DeleteAsync(TId id, CancellationToken cancellation = default)
    {
        await Repository.DeleteAsync(id, cancellation);
    }
}
