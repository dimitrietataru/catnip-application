namespace CatNip.Application.Test.Services.Abstractions;

public interface IAceServiceTests : ICrudServiceTests
{
    Task GivenGetFilteredWhenDataExistsThenReturnsData();
    Task GivenCountFilteredWhenDataExistsThenReturnsData();
}
