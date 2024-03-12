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
    public virtual async Task GivenCreateWhenInputIsValidThenCreatesData()
    {
        // Arrange
        ArrangeCreateOnSuccess();

        // Act
        var result = await Service.CreateAsync(It.IsAny<TModel>(), It.IsAny<CancellationToken>());

        // Assert
        AssertCreateOnSuccess(result);
    }

    protected virtual void ArrangeCreateOnSuccess()
    {
        RepositoryMock
            .Setup(_ => _.CreateAsync(It.IsAny<TModel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Activator.CreateInstance<TModel>())
            .Verifiable();
    }

    protected virtual void AssertCreateOnSuccess(TModel model)
    {
        model.Should().NotBeNull().And.BeOfType<TModel>();

        RepositoryMock.Verify(
            _ => _.CreateAsync(It.IsAny<TModel>(), It.IsAny<CancellationToken>()), Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }
}
