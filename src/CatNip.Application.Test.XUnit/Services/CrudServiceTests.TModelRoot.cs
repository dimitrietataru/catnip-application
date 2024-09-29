using CatNip.Application.Services;
using CatNip.Domain.Models.Interfaces;
using CatNip.Domain.Repositories;

namespace CatNip.Application.Test.XUnit.Services;

public abstract class CrudServiceTests<TService, TRepository, TModel, TModelRoot, TId>
    : CrudServiceTests<TService, TRepository, TModel, TId>
    where TService : CrudService<TRepository, TModel, TId>
    where TRepository : class, ICrudRepository<TModel, TId>
    where TModel : IModel<TId>
    where TModelRoot : IModel
    where TId : IEquatable<TId>
{
    [Fact]
    public virtual async Task GivenGetAllTModelRootWhenDataExistsThenReturnsData()
    {
        await base.GivenGetAllTWhenDataExistsThenReturnsData<TModelRoot>();
    }
}
