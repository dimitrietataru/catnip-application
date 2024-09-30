namespace CatNip.Application.Test.Services.Abstractions;

public interface ICrudServiceTests
{
    Task GivenGetAllWhenDataExistsThenReturnsData();

    Task GivenCountWhenDataExistsThenReturnsCount();

    Task GivenGetByIdWhenDataExistsThenReturnsData();
    Task GivenGetByIdWhenDataNotFoundThenThrowsException();

    Task GivenExistsWhenDataExistsThenReturnsTrue();
    Task GivenExistsWhenDataNotFoundThenReturnsFalse();

    Task GivenCreateWhenInputIsValidThenCreatesData();

    Task GivenUpdateWhenDataExistsThenUpdatesData();
    Task GivenUpdateWhenDataNotFoundThenThrowsException();

    Task GivenDeleteWhenDataExistsThenDeletesData();
    Task GivenDeleteWhenDataNotFoundThenThrowsException();
}
