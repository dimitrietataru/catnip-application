using CatNip.Application.Services;
using CatNip.Domain.Exceptions;
using CatNip.Domain.Models.Interfaces;
using CatNip.Domain.Repositories;

namespace CatNip.Application.Test.Services;

public abstract partial class BaseCrudServiceTests<TService, TRepository, TModel, TId>
    where TService : CrudService<TRepository, TModel, TId>
    where TRepository : class, ICrudRepository<TModel, TId>
    where TModel : IModel<TId>
    where TId : IEquatable<TId>
{
    public virtual async Task GivenDeleteWhenDataExistsThenDeletesData()
    {
        // Arrange
        ArrangeDeleteOnSuccess();

        // Act
        await Service.DeleteAsync(It.IsAny<TId>(), It.IsAny<CancellationToken>());

        // Assert
        AssertDeleteOnSuccess();
    }

    public virtual async Task GivenDeleteWhenDataNotFoundThenThrowsException()
    {
        // Arrange
        ArrangeDeleteOnNotFound();

        // Act
        var action = () => Service.DeleteAsync(It.IsAny<TId>(), It.IsAny<CancellationToken>());

        // Assert
        await action.Should().ThrowAsync<DataNotFoundException>();
        AssertDeleteOnNotFound();
    }

    protected virtual void ArrangeDeleteOnSuccess()
    {
        RepositoryMock
            .Setup(_ => _.DeleteAsync(It.IsAny<TId>(), It.IsAny<CancellationToken>()))
            .Verifiable();
    }

    protected virtual void AssertDeleteOnSuccess()
    {
        RepositoryMock.Verify(
            _ => _.DeleteAsync(It.IsAny<TId>(), It.IsAny<CancellationToken>()), Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }

    protected virtual void ArrangeDeleteOnNotFound()
    {
        RepositoryMock
            .Setup(_ => _.DeleteAsync(It.IsAny<TId>(), It.IsAny<CancellationToken>()))
            .Throws<DataNotFoundException>()
            .Verifiable();
    }

    protected virtual void AssertDeleteOnNotFound()
    {
        RepositoryMock.Verify(
            _ => _.DeleteAsync(It.IsAny<TId>(), It.IsAny<CancellationToken>()), Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }
}
