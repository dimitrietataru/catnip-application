using CatNip.Application.Services;
using CatNip.Domain.Models.Interfaces;
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
    public virtual async Task GivenCountFilteredWhenDataExistsThenReturnsData()
    {
        // Arrange
        ArrangeCountFilteredOnSuccess();

        // Act
        int count = await Service.CountAsync(It.IsAny<TFiltering>(), It.IsAny<CancellationToken>());

        // Assert
        AssertCountFilteredOnSuccess(count);
    }

    protected virtual void ArrangeCountFilteredOnSuccess()
    {
        RepositoryMock
            .Setup(_ => _.CountAsync(It.IsAny<TFiltering>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(0)
            .Verifiable();
    }

    protected virtual void AssertCountFilteredOnSuccess(int count)
    {
        count.Should().BeGreaterThanOrEqualTo(0);

        RepositoryMock.Verify(
            _ => _.CountAsync(It.IsAny<TFiltering>(), It.IsAny<CancellationToken>()), Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }
}
