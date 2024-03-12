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
    public virtual async Task GivenGetFilteredWhenDataExistsThenReturnsData()
    {
        // Arrange
        ArrangeGetFilteredOnSuccess();

        // Act
        var result = await Service.GetAsync(
            It.IsAny<QueryRequest<TFiltering>>(), It.IsAny<CancellationToken>());

        // Assert
        AssertGetFilteredOnSuccess(result);
    }

    protected virtual void ArrangeGetFilteredOnSuccess()
    {
        RepositoryMock
            .Setup(_ => _.GetAsync(It.IsAny<QueryRequest<TFiltering>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(It.IsAny<QueryResponse<TModel>>())
            .Verifiable();
    }

    protected virtual void AssertGetFilteredOnSuccess(QueryResponse<TModel> result)
    {
        RepositoryMock.Verify(
            _ => _.GetAsync(It.IsAny<QueryRequest<TFiltering>>(), It.IsAny<CancellationToken>()),
            Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }
}
