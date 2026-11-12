using CatNip.Application.Services;
using CatNip.Domain.Exceptions;
using CatNip.Domain.Models.Interfaces;
using CatNip.Domain.Repositories;

namespace CatNip.Application.Test.Services;

public abstract class BaseCrudServiceTests<TService, TRepository, TModel, TId>
    : BaseServiceTests<TService, TRepository, TModel>
    where TService : CrudService<TRepository, TModel, TId>
    where TRepository : class, ICrudRepository<TModel, TId>
    where TModel : ICrudModel<TId>
    where TId : IEquatable<TId>
{
    public virtual async Task GivenGetByIdWhenDataExistsThenReturnsData()
    {
        // Arrange
        RepositoryMock
            .Setup(_ => _.GetByIdAsync(It.IsAny<TId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Activator.CreateInstance<TModel>())
            .Verifiable();

        // Act
        var result = await Service.GetByIdAsync(It.IsAny<TId>(), It.IsAny<CancellationToken>());

        // Assert
        result.Should().NotBeNull().And.BeOfType<TModel>();
        RepositoryMock.Verify(_ => _.GetByIdAsync(It.IsAny<TId>(), It.IsAny<CancellationToken>()), Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }

    public virtual async Task GivenGetByIdWhenDataNotFoundThenThrowsException()
    {
        // Arrange
        RepositoryMock
            .Setup(_ => _.GetByIdAsync(It.IsAny<TId>(), It.IsAny<CancellationToken>()))
            .Throws<DataNotFoundException>()
            .Verifiable();

        // Act
        var action = () => Service.GetByIdAsync(It.IsAny<TId>(), It.IsAny<CancellationToken>());

        // Assert
        await action.Should().ThrowAsync<DataNotFoundException>();
        RepositoryMock.Verify(_ => _.GetByIdAsync(It.IsAny<TId>(), It.IsAny<CancellationToken>()), Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }

    public virtual async Task GivenExistsWhenDataExistsThenReturnsTrue()
    {
        // Arrange
        RepositoryMock
            .Setup(_ => _.ExistsAsync(It.IsAny<TId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true)
            .Verifiable();

        // Act
        bool exists = await Service.ExistsAsync(It.IsAny<TId>(), It.IsAny<CancellationToken>());

        // Assert
        exists.Should().BeTrue();
        RepositoryMock.Verify(_ => _.ExistsAsync(It.IsAny<TId>(), It.IsAny<CancellationToken>()), Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }

    public virtual async Task GivenExistsWhenDataNotFoundThenReturnsFalse()
    {
        // Arrange
        RepositoryMock
            .Setup(_ => _.ExistsAsync(It.IsAny<TId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false)
            .Verifiable();

        // Act
        bool exists = await Service.ExistsAsync(It.IsAny<TId>(), It.IsAny<CancellationToken>());

        // Assert
        exists.Should().BeFalse();
        RepositoryMock.Verify(_ => _.ExistsAsync(It.IsAny<TId>(), It.IsAny<CancellationToken>()), Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }

    public virtual async Task GivenUpdateByIdWhenDataExistsThenUpdatesData()
    {
        // Arrange
        RepositoryMock
            .Setup(_ => _.UpdateAsync(It.IsAny<TId>(), It.IsAny<TModel>(), It.IsAny<CancellationToken>()))
            .Verifiable();

        // Act
        await Service.UpdateAsync(It.IsAny<TId>(), It.IsAny<TModel>(), It.IsAny<CancellationToken>());

        // Assert
        RepositoryMock.Verify(_ => _.UpdateAsync(It.IsAny<TId>(), It.IsAny<TModel>(), It.IsAny<CancellationToken>()), Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }

    public virtual async Task GivenUpdateByIdWhenDataNotFoundThenThrowsException()
    {
        // Arrange
        RepositoryMock
            .Setup(_ => _.UpdateAsync(It.IsAny<TId>(), It.IsAny<TModel>(), It.IsAny<CancellationToken>()))
            .Throws<DataNotFoundException>()
            .Verifiable();

        // Act
        var action = () => Service.UpdateAsync(It.IsAny<TId>(), It.IsAny<TModel>(), It.IsAny<CancellationToken>());

        // Assert
        await action.Should().ThrowAsync<DataNotFoundException>();
        RepositoryMock.Verify(_ => _.UpdateAsync(It.IsAny<TId>(), It.IsAny<TModel>(), It.IsAny<CancellationToken>()), Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }

    public virtual async Task GivenDeleteByIdWhenDataExistsThenDeletesData()
    {
        // Arrange
        RepositoryMock
            .Setup(_ => _.DeleteAsync(It.IsAny<TId>(), It.IsAny<CancellationToken>()))
            .Verifiable();

        // Act
        await Service.DeleteAsync(It.IsAny<TId>(), It.IsAny<CancellationToken>());

        // Assert
        RepositoryMock.Verify(_ => _.DeleteAsync(It.IsAny<TId>(), It.IsAny<CancellationToken>()), Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }

    public virtual async Task GivenDeleteByIdWhenDataNotFoundThenThrowsException()
    {
        // Arrange
        RepositoryMock
            .Setup(_ => _.DeleteAsync(It.IsAny<TId>(), It.IsAny<CancellationToken>()))
            .Throws<DataNotFoundException>()
            .Verifiable();

        // Act
        var action = () => Service.DeleteAsync(It.IsAny<TId>(), It.IsAny<CancellationToken>());

        // Assert
        await action.Should().ThrowAsync<DataNotFoundException>();
        RepositoryMock.Verify(_ => _.DeleteAsync(It.IsAny<TId>(), It.IsAny<CancellationToken>()), Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }
}
