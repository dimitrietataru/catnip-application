namespace CatNip.Application.Test.Services.Abstractions;

public interface IExchangeServiceTests
{
    Task GivenImportCsvWhenDataIsValidThenImportsData();
    Task GivenImportCsvWhenFileParseFailsThenReturnsFailure();
    Task GivenImportCsvWhenCsvMapNotFoundThenReturnsFailure();
    Task GivenImportCsvWhenValidationFailsThenReturnsFailure();
    Task GivenImportCsvWhenDataIntegrityFailsThenReturnsFailure();

    Task GivenImportExcelWhenDataIsValidThenImportsData();
    Task GivenImportExcelWhenFileParseFailsThenReturnsFailure();
    Task GivenImportExcelWhenExcelMapNotFoundThenReturnsFailure();
    Task GivenImportExcelWhenValidationFailsThenReturnsFailure();
    Task GivenImportExcelWhenDataIntegrityFailsThenReturnsFailure();
}
