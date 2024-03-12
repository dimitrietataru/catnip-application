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
    public virtual async Task GivenGetAllWhenDataExistsThenReturnsData()
    {
        // Arrange
        ArrangeGetAllOnSuccess();

        // Act
        var result = await Service.GetAllAsync(It.IsAny<CancellationToken>());

        // Assert
        AssertGetAllOnSuccess(result);
    }

    protected virtual void ArrangeGetAllOnSuccess()
    {
        RepositoryMock
            .Setup(_ => _.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([])
            .Verifiable();
    }

    protected virtual void AssertGetAllOnSuccess(IEnumerable<TModel> result)
    {
        result.Should().NotBeNull().And.BeAssignableTo<IEnumerable<TModel>>();

        RepositoryMock.Verify(_ => _.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }
}
