using CatNip.Application.Services;
using CatNip.Domain.Models.Interfaces;
using CatNip.Domain.Repositories;

namespace CatNip.Application.Test.Services;

public abstract partial class BaseCrudServiceTests<TService, TRepository, TModel, TId>
    where TService : CrudService<TRepository, TModel, TId>
    where TRepository : class, ICrudRepository<TModel, TId>
    where TModel : IModel<TId>
    where TId : IEquatable<TId>
{
    public virtual async Task GivenCountWhenDataExistsThenReturnsCount()
    {
        // Arrange
        ArrangeCountOnSuccess();

        // Act
        int count = await Service.CountAsync(It.IsAny<CancellationToken>());

        // Assert
        AssertCountOnSuccess(count);
    }

    protected virtual void ArrangeCountOnSuccess()
    {
        RepositoryMock
            .Setup(_ => _.CountAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(0)
            .Verifiable();
    }

    protected virtual void AssertCountOnSuccess(int count)
    {
        count.Should().BeGreaterThanOrEqualTo(0);

        RepositoryMock.Verify(_ => _.CountAsync(It.IsAny<CancellationToken>()), Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }
}
