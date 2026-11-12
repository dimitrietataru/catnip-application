using CatNip.Application.Services;
using CatNip.Application.Test.Services;
using CatNip.Domain.Models.Interfaces;
using CatNip.Domain.Repositories;

namespace CatNip.Application.Test.XUnit.Services;

public abstract class ServiceTests<TService, TRepository, TModel> : BaseServiceTests<TService, TRepository, TModel>
    where TService : Service<TRepository, TModel>
    where TRepository : class, IRepository<TModel>
    where TModel : IModel
{
    [Fact]
    public override async Task GivenGetAllWhenDataExistsThenReturnsData()
    {
        await base.GivenGetAllWhenDataExistsThenReturnsData();
    }

    [Fact]
    public override async Task GivenCountWhenDataExistsThenReturnsCount()
    {
        await base.GivenCountWhenDataExistsThenReturnsCount();
    }

    [Fact]
    public override async Task GivenCreateWhenInputIsValidThenCreatesData()
    {
        await base.GivenCreateWhenInputIsValidThenCreatesData();
    }

    [Fact]
    public override async Task GivenUpdateWhenDataExistsThenUpdatesData()
    {
        await base.GivenUpdateWhenDataExistsThenUpdatesData();
    }

    [Fact]
    public override async Task GivenUpdateWhenDataNotFoundThenThrowsException()
    {
        await base.GivenUpdateWhenDataNotFoundThenThrowsException();
    }

    [Fact]
    public override async Task GivenDeleteWhenDataExistsThenDeletesData()
    {
        await base.GivenDeleteWhenDataExistsThenDeletesData();
    }

    [Fact]
    public override async Task GivenDeleteWhenDataNotFoundThenThrowsException()
    {
        await base.GivenDeleteWhenDataNotFoundThenThrowsException();
    }
}
