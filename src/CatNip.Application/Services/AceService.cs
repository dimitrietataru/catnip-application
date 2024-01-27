using CatNip.Domain.Models.Interfaces;
using CatNip.Domain.Query;
using CatNip.Domain.Query.Filtering;
using CatNip.Domain.Repositories;
using CatNip.Domain.Services;

namespace CatNip.Application.Services;

public abstract class AceService<TRepository, TModel, TId, TFiltering>
    : CrudService<TRepository, TModel, TId>, IAceService<TModel, TId, TFiltering>
    where TRepository : IAceRepository<TModel, TId, TFiltering>
    where TModel : IModel<TId>
    where TId : IEquatable<TId>
    where TFiltering : IFilteringRequest
{
    protected AceService(TRepository repository)
        : base(repository)
    {
    }

    public virtual async Task<QueryResponse<TModel>> GetAsync(
        QueryRequest<TFiltering> request, CancellationToken cancellation = default)
    {
        var result = await Repository.GetAsync(request, cancellation);

        return result;
    }

    public virtual async Task<int> CountAsync(
        QueryRequest<TFiltering> request, CancellationToken cancellation = default)
    {
        int count = await Repository.CountAsync(request, cancellation);

        return count;
    }
}
