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
    public virtual async Task GivenGetFilteredTWhenDataExistsThenReturnsData<TModelRoot>()
        where TModelRoot : IModel<TId>
    {
        // Arrange
        ArrangeGetFilteredTOnSuccess<TModelRoot>();

        // Act
        var result = await Service.GetAsync<TModelRoot>(
            It.IsAny<QueryRequest<TFiltering>>(), It.IsAny<CancellationToken>());

        // Assert
        AssertGetFilteredTOnSuccess(result);
    }

    protected virtual void ArrangeGetFilteredTOnSuccess<TModelRoot>()
        where TModelRoot : IModel<TId>
    {
        RepositoryMock
            .Setup(_ => _.GetAsync<TModelRoot>(
                It.IsAny<QueryRequest<TFiltering>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(It.IsAny<QueryResponse<TModelRoot>>())
            .Verifiable();
    }

    protected virtual void AssertGetFilteredTOnSuccess<TModelRoot>(QueryResponse<TModelRoot> result)
        where TModelRoot : IModel<TId>
    {
        RepositoryMock.Verify(
            _ => _.GetAsync<TModelRoot>(It.IsAny<QueryRequest<TFiltering>>(), It.IsAny<CancellationToken>()),
            Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }
}
