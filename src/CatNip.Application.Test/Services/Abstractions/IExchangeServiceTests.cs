namespace CatNip.Application.Test.Services.Abstractions;

public interface IExchangeServiceTests
{
    Task GivenImportWhenDataIsValidThenImportsData();
    Task GivenImportWhenFileParseFailsThenReturnsFailure();
    Task GivenImportWhenValidationFailsThenReturnsFailure();
    Task GivenImportWhenDataIntegrityFailsThenReturnsFailure();
}
