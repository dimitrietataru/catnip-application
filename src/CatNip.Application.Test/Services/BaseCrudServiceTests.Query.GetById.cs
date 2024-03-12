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
    public virtual async Task GivenGetByIdWhenDataExistsThenReturnsData()
    {
        // Arrange
        ArrangeGetByIdOnSuccess();

        // Act
        var result = await Service.GetByIdAsync(It.IsAny<TId>(), It.IsAny<CancellationToken>());

        // Assert
        AssertGetByIdOnSuccess(result);
    }

    public virtual async Task GivenGetByIdWhenDataNotFoundThenThrowsException()
    {
        // Arrange
        ArrangeGetByIdOnNotFound();

        // Act
        var action = () => Service.GetByIdAsync(It.IsAny<TId>(), It.IsAny<CancellationToken>());

        // Assert
        await action.Should().ThrowAsync<DataNotFoundException>();
        AssertGetByIdOnNotFound();
    }

    protected virtual void ArrangeGetByIdOnSuccess()
    {
        RepositoryMock
            .Setup(_ => _.GetByIdAsync(It.IsAny<TId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Activator.CreateInstance<TModel>())
            .Verifiable();
    }

    protected virtual void AssertGetByIdOnSuccess(TModel model)
    {
        model.Should().NotBeNull().And.BeOfType<TModel>();

        RepositoryMock.Verify(
            _ => _.GetByIdAsync(It.IsAny<TId>(), It.IsAny<CancellationToken>()), Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }

    protected virtual void ArrangeGetByIdOnNotFound()
    {
        RepositoryMock
            .Setup(_ => _.GetByIdAsync(It.IsAny<TId>(), It.IsAny<CancellationToken>()))
            .Throws<DataNotFoundException>()
            .Verifiable();
    }

    protected virtual void AssertGetByIdOnNotFound()
    {
        RepositoryMock.Verify(
            _ => _.GetByIdAsync(It.IsAny<TId>(), It.IsAny<CancellationToken>()), Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }
}
