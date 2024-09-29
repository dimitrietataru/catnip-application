using CatNip.Application.Services;
using CatNip.Domain.Models.Interfaces;
using CatNip.Domain.Query.Filtering;
using CatNip.Domain.Repositories;

namespace CatNip.Application.Test.XUnit.Services;

public abstract class AceServiceTests<TService, TRepository, TModel, TModelRoot, TId, TFiltering>
    : AceServiceTests<TService, TRepository, TModel, TId, TFiltering>
    where TService : AceService<TRepository, TModel, TId, TFiltering>
    where TRepository : class, IAceRepository<TModel, TId, TFiltering>
    where TModel : IModel<TId>
    where TModelRoot : IModel<TId>
    where TId : IEquatable<TId>
    where TFiltering : IFilteringRequest
{
    [Fact]
    public virtual async Task GivenGetFilteredTModelRootWhenDataExistsThenReturnsData()
    {
        await base.GivenGetFilteredTWhenDataExistsThenReturnsData<TModelRoot>();
    }

    [Fact]
    public virtual async Task GivenGetAllTModelRootWhenDataExistsThenReturnsData()
    {
        await base.GivenGetAllTWhenDataExistsThenReturnsData<TModelRoot>();
    }
}
