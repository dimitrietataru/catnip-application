using CatNip.Domain.Models.Interfaces;
using CatNip.Domain.Repositories;
using CatNip.Domain.Services;

namespace CatNip.Application.Services;

public abstract class Service<TRepository, TModel> : IService<TModel>
    where TRepository : IRepository<TModel>
    where TModel : IModel
{
    protected Service(TRepository repository)
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

    public virtual async Task<TModel> CreateAsync(TModel model, CancellationToken cancellation = default)
    {
        var result = await Repository.CreateAsync(model, cancellation);

        return result;
    }

    public virtual async Task UpdateAsync(TModel model, CancellationToken cancellation = default)
    {
        await Repository.UpdateAsync(model, cancellation);
    }

    public virtual async Task DeleteAsync(TModel model, CancellationToken cancellation = default)
    {
        await Repository.DeleteAsync(model, cancellation);
    }
}
