using CatNip.Application.Services;
using CatNip.Domain.Models.Interfaces;
using CatNip.Domain.Query;
using CatNip.Domain.Query.Filtering;
using CatNip.Domain.Repositories;

namespace CatNip.Application.Test.Services;

public abstract partial class BaseAceServiceTests<TService, TRepository, TModel, TId, TFiltering>
    : BaseCrudServiceTests<TService, TRepository, TModel, TId>
    where TService : AceService<TRepository, TModel, TId, TFiltering>
    where TRepository : class, IAceRepository<TModel, TId, TFiltering>
    where TModel : IModel<TId>
    where TId : IEquatable<TId>
    where TFiltering : IFilteringRequest
{
    public virtual async Task GivenGetFilteredWhenDataExistsThenReturnsData<TModelRoot>()
        where TModelRoot : IModel<TId>
    {
        // Arrange
        ArrangeGetFilteredOnSuccess<TModelRoot>();

        // Act
        var result = await Service.GetAsync<TModelRoot>(
            It.IsAny<QueryRequest<TFiltering>>(), It.IsAny<CancellationToken>());

        // Assert
        AssertGetFilteredOnSuccess(result);
    }

    protected virtual void ArrangeGetFilteredOnSuccess<TModelRoot>()
        where TModelRoot : IModel<TId>
    {
        RepositoryMock
            .Setup(_ => _.GetAsync<TModelRoot>(
                It.IsAny<QueryRequest<TFiltering>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(It.IsAny<QueryResponse<TModelRoot>>())
            .Verifiable();
    }

    protected virtual void AssertGetFilteredOnSuccess<TModelRoot>(QueryResponse<TModelRoot> result)
        where TModelRoot : IModel<TId>
    {
        RepositoryMock.Verify(
            _ => _.GetAsync(It.IsAny<QueryRequest<TFiltering>>(), It.IsAny<CancellationToken>()),
            Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }
}
