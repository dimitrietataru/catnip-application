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
    public virtual async Task GivenGetAllTWhenDataExistsThenReturnsData<TModelRoot>()
        where TModelRoot : IModel
    {
        // Arrange
        ArrangeGetAllTOnSuccess<TModelRoot>();

        // Act
        var result = await Service.GetAllAsync<TModelRoot>(It.IsAny<CancellationToken>());

        // Assert
        AssertGetAllTOnSuccess(result);
    }

    protected virtual void ArrangeGetAllTOnSuccess<TModelRoot>()
        where TModelRoot : IModel
    {
        RepositoryMock
            .Setup(_ => _.GetAllAsync<TModelRoot>(It.IsAny<CancellationToken>()))
            .ReturnsAsync([])
            .Verifiable();
    }

    protected virtual void AssertGetAllTOnSuccess<TModelRoot>(IEnumerable<TModelRoot> result)
        where TModelRoot : IModel
    {
        result.Should().NotBeNull().And.BeAssignableTo<IEnumerable<TModelRoot>>();

        RepositoryMock.Verify(
            _ => _.GetAllAsync<TModelRoot>(It.IsAny<CancellationToken>()),
            Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }
}
