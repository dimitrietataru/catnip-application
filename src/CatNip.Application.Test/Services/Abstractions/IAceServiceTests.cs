namespace CatNip.Application.Test.Services.Abstractions;

public interface IAceServiceTests : ICrudServiceTests, IExchangeServiceTests
{
    Task GivenGetFilteredWhenDataExistsThenReturnsData();
    Task GivenCountFilteredWhenDataExistsThenReturnsData();
}
