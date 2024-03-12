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
    public virtual async Task GivenExistsWhenDataExistsThenReturnsTrue()
    {
        // Arrange
        ArrangeExistsOnSuccess();

        // Act
        bool exists = await Service.ExistsAsync(It.IsAny<TId>(), It.IsAny<CancellationToken>());

        // Assert
        AssertExistsOnSuccess(exists);
    }

    public virtual async Task GivenExistsWhenDataNotFoundThenReturnsFalse()
    {
        // Arrange
        ArrangeExistsOnNotFound();

        // Act
        bool exists = await Service.ExistsAsync(It.IsAny<TId>(), It.IsAny<CancellationToken>());

        // Assert
        AssertExistsOnNotFound(exists);
    }

    protected virtual void ArrangeExistsOnSuccess()
    {
        RepositoryMock
            .Setup(_ => _.ExistsAsync(It.IsAny<TId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true)
            .Verifiable();
    }

    protected virtual void AssertExistsOnSuccess(bool exists)
    {
        exists.Should().BeTrue();

        RepositoryMock.Verify(
            _ => _.ExistsAsync(It.IsAny<TId>(), It.IsAny<CancellationToken>()), Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }

    protected virtual void ArrangeExistsOnNotFound()
    {
        RepositoryMock
            .Setup(_ => _.ExistsAsync(It.IsAny<TId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false)
            .Verifiable();
    }

    protected virtual void AssertExistsOnNotFound(bool exists)
    {
        exists.Should().BeFalse();

        RepositoryMock.Verify(
            _ => _.ExistsAsync(It.IsAny<TId>(), It.IsAny<CancellationToken>()), Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }
}
