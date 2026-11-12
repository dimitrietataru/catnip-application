using CatNip.Application.Services;
using CatNip.Domain.Exceptions;
using CatNip.Domain.Models.Interfaces;
using CatNip.Domain.Repositories;

namespace CatNip.Application.Test.Services;

public abstract class BaseServiceTests<TService, TRepository, TModel>
    where TService : Service<TRepository, TModel>
    where TRepository : class, IRepository<TModel>
    where TModel : IModel
{
    protected abstract TService Service { get; }
    protected abstract Mock<TRepository> RepositoryMock { get; }

    public virtual async Task GivenGetAllWhenDataExistsThenReturnsData()
    {
        // Arrange
        RepositoryMock
            .Setup(_ => _.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([])
            .Verifiable();

        // Act
        var result = await Service.GetAllAsync(It.IsAny<CancellationToken>());

        // Assert
        result.Should().NotBeNull().And.BeAssignableTo<IEnumerable<TModel>>();
        RepositoryMock.Verify(_ => _.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }

    public virtual async Task GivenCountWhenDataExistsThenReturnsCount()
    {
        // Arrange
        RepositoryMock
            .Setup(_ => _.CountAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(0)
            .Verifiable();

        // Act
        int count = await Service.CountAsync(It.IsAny<CancellationToken>());

        // Assert
        count.Should().BeGreaterThanOrEqualTo(0);
        RepositoryMock.Verify(_ => _.CountAsync(It.IsAny<CancellationToken>()), Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }

    public virtual async Task GivenCreateWhenInputIsValidThenCreatesData()
    {
        // Arrange
        RepositoryMock
            .Setup(_ => _.CreateAsync(It.IsAny<TModel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Activator.CreateInstance<TModel>())
            .Verifiable();

        // Act
        var result = await Service.CreateAsync(It.IsAny<TModel>(), It.IsAny<CancellationToken>());

        // Assert
        result.Should().NotBeNull().And.BeOfType<TModel>();
        RepositoryMock.Verify(_ => _.CreateAsync(It.IsAny<TModel>(), It.IsAny<CancellationToken>()), Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }

    public virtual async Task GivenUpdateWhenDataExistsThenUpdatesData()
    {
        // Arrange
        RepositoryMock
            .Setup(_ => _.UpdateAsync(It.IsAny<TModel>(), It.IsAny<CancellationToken>()))
            .Verifiable();

        // Act
        await Service.UpdateAsync(It.IsAny<TModel>(), It.IsAny<CancellationToken>());

        // Assert
        RepositoryMock.Verify(_ => _.UpdateAsync(It.IsAny<TModel>(), It.IsAny<CancellationToken>()), Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }

    public virtual async Task GivenUpdateWhenDataNotFoundThenThrowsException()
    {
        // Arrange
        RepositoryMock
            .Setup(_ => _.UpdateAsync(It.IsAny<TModel>(), It.IsAny<CancellationToken>()))
            .Throws<DataNotFoundException>()
            .Verifiable();

        // Act
        var action = () => Service.UpdateAsync(It.IsAny<TModel>(), It.IsAny<CancellationToken>());

        // Assert
        await action.Should().ThrowAsync<DataNotFoundException>();
        RepositoryMock.Verify(_ => _.UpdateAsync(It.IsAny<TModel>(), It.IsAny<CancellationToken>()), Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }

    public virtual async Task GivenDeleteWhenDataExistsThenDeletesData()
    {
        // Arrange
        RepositoryMock
            .Setup(_ => _.DeleteAsync(It.IsAny<TModel>(), It.IsAny<CancellationToken>()))
            .Verifiable();

        // Act
        await Service.DeleteAsync(It.IsAny<TModel>(), It.IsAny<CancellationToken>());

        // Assert
        RepositoryMock.Verify(_ => _.DeleteAsync(It.IsAny<TModel>(), It.IsAny<CancellationToken>()), Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }

    public virtual async Task GivenDeleteWhenDataNotFoundThenThrowsException()
    {
        // Arrange
        RepositoryMock
            .Setup(_ => _.DeleteAsync(It.IsAny<TModel>(), It.IsAny<CancellationToken>()))
            .Throws<DataNotFoundException>()
            .Verifiable();

        // Act
        var action = () => Service.DeleteAsync(It.IsAny<TModel>(), It.IsAny<CancellationToken>());

        // Assert
        await action.Should().ThrowAsync<DataNotFoundException>();
        RepositoryMock.Verify(_ => _.DeleteAsync(It.IsAny<TModel>(), It.IsAny<CancellationToken>()), Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }
}
