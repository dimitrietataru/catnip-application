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
    public virtual async Task GivenUpdateWhenDataExistsThenUpdatesData()
    {
        // Arrange
        ArrangeUpdateOnSuccess();

        // Act
        await Service.UpdateAsync(
            It.IsAny<TId>(), It.IsAny<TModel>(), It.IsAny<CancellationToken>());

        // Assert
        AssertUpdateOnSuccess();
    }

    public virtual async Task GivenUpdateWhenDataNotFoundThenThrowsException()
    {
        // Arrange
        ArrangeUpdateOnNotFound();

        // Act
        var action = () => Service.UpdateAsync(
            It.IsAny<TId>(), It.IsAny<TModel>(), It.IsAny<CancellationToken>());

        // Assert
        await action.Should().ThrowAsync<DataNotFoundException>();
        AssertUpdateOnNotFound();
    }

    protected virtual void ArrangeUpdateOnSuccess()
    {
        RepositoryMock
            .Setup(_ => _.UpdateAsync(
                It.IsAny<TId>(), It.IsAny<TModel>(), It.IsAny<CancellationToken>()))
            .Verifiable();
    }

    protected virtual void AssertUpdateOnSuccess()
    {
        RepositoryMock.Verify(
            _ => _.UpdateAsync(It.IsAny<TId>(), It.IsAny<TModel>(), It.IsAny<CancellationToken>()),
            Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }

    protected virtual void ArrangeUpdateOnNotFound()
    {
        RepositoryMock
            .Setup(_ => _.UpdateAsync(
                It.IsAny<TId>(), It.IsAny<TModel>(), It.IsAny<CancellationToken>()))
            .Throws<DataNotFoundException>()
            .Verifiable();
    }

    protected virtual void AssertUpdateOnNotFound()
    {
        RepositoryMock.Verify(
            _ => _.UpdateAsync(It.IsAny<TId>(), It.IsAny<TModel>(), It.IsAny<CancellationToken>()),
            Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }
}
