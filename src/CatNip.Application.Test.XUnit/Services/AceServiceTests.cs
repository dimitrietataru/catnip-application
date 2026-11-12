using CatNip.Application.Services;
using CatNip.Application.Test.Services;
using CatNip.Domain.ImportExport.Csv;
using CatNip.Domain.ImportExport.Excel;
using CatNip.Domain.Models.Interfaces;
using CatNip.Domain.Query.Filtering;
using CatNip.Domain.Repositories;

namespace CatNip.Application.Test.XUnit.Services;

public abstract class AceServiceTests<TService, TRepository, TModel, TId, TModelRoot, TFiltering, TExchange>
    : AceServiceTests<TService, TRepository, TModel, TId, TId, TModelRoot, TFiltering, TExchange>
    where TService : AceService<TRepository, TModel, TId, TModelRoot, TFiltering, TExchange>
    where TRepository : class, IAceRepository<TModel, TId, TModelRoot, TFiltering, TExchange>
    where TModel : IAceModel<TId>
    where TId : IEquatable<TId>
    where TModelRoot : ICrudModel<TId>
    where TFiltering : IFilteringRequest
    where TExchange : ICsvMappable, IExcelMappable
{
}

public abstract class AceServiceTests<TService, TRepository, TModel, TId, TTraceId, TModelRoot, TFiltering, TExchange>
    : BaseAceServiceTests<TService, TRepository, TModel, TId, TTraceId, TModelRoot, TFiltering, TExchange>
    where TService : AceService<TRepository, TModel, TId, TTraceId, TModelRoot, TFiltering, TExchange>
    where TRepository : class, IAceRepository<TModel, TId, TTraceId, TModelRoot, TFiltering, TExchange>
    where TModel : IAceModel<TId, TTraceId>
    where TId : IEquatable<TId>
    where TTraceId : IEquatable<TTraceId>
    where TModelRoot : ICrudModel<TId>
    where TFiltering : IFilteringRequest
    where TExchange : ICsvMappable, IExcelMappable
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

    [Fact]
    public override async Task GivenGetByIdWhenDataExistsThenReturnsData()
    {
        await base.GivenGetByIdWhenDataExistsThenReturnsData();
    }

    [Fact]
    public override async Task GivenGetByIdWhenDataNotFoundThenThrowsException()
    {
        await base.GivenGetByIdWhenDataNotFoundThenThrowsException();
    }

    [Fact]
    public override async Task GivenExistsWhenDataExistsThenReturnsTrue()
    {
        await base.GivenExistsWhenDataExistsThenReturnsTrue();
    }

    [Fact]
    public override async Task GivenExistsWhenDataNotFoundThenReturnsFalse()
    {
        await base.GivenExistsWhenDataNotFoundThenReturnsFalse();
    }

    [Fact]
    public override async Task GivenUpdateByIdWhenDataExistsThenUpdatesData()
    {
        await base.GivenUpdateByIdWhenDataExistsThenUpdatesData();
    }

    [Fact]
    public override async Task GivenUpdateByIdWhenDataNotFoundThenThrowsException()
    {
        await base.GivenUpdateByIdWhenDataNotFoundThenThrowsException();
    }

    [Fact]
    public override async Task GivenDeleteByIdWhenDataExistsThenDeletesData()
    {
        await base.GivenDeleteByIdWhenDataExistsThenDeletesData();
    }

    [Fact]
    public override async Task GivenDeleteByIdWhenDataNotFoundThenThrowsException()
    {
        await base.GivenDeleteByIdWhenDataNotFoundThenThrowsException();
    }

    [Fact]
    public override async Task GivenGetByFilterWhenDataExistsThenReturnsData()
    {
        await base.GivenGetByFilterWhenDataExistsThenReturnsData();
    }

    [Fact]
    public override async Task GivenCountByFilterWhenDataExistsThenReturnsCount()
    {
        await base.GivenCountByFilterWhenDataExistsThenReturnsCount();
    }

    [Fact]
    public override async Task GivenExistsByFilterWhenDataExistsThenReturnsTrue()
    {
        await base.GivenExistsByFilterWhenDataExistsThenReturnsTrue();
    }

    [Fact]
    public override async Task GivenExistsByFilterWhenDataNotFoundThenReturnsFalse()
    {
        await base.GivenExistsByFilterWhenDataNotFoundThenReturnsFalse();
    }

    [Fact]
    public override async Task GivenImportCsvWhenDataIsValidThenImportsData()
    {
        await base.GivenImportCsvWhenDataIsValidThenImportsData();
    }

    [Fact]
    public override async Task GivenImportCsvWhenFileParseFailsThenReturnsFailure()
    {
        await base.GivenImportCsvWhenFileParseFailsThenReturnsFailure();
    }

    [Fact]
    public override async Task GivenImportCsvWhenCsvMapNotFoundThenReturnsFailure()
    {
        await base.GivenImportCsvWhenCsvMapNotFoundThenReturnsFailure();
    }

    [Fact]
    public override async Task GivenImportCsvWhenValidationFailsThenReturnsFailure()
    {
        await base.GivenImportCsvWhenValidationFailsThenReturnsFailure();
    }

    [Fact]
    public override async Task GivenImportCsvWhenDataIntegrityFailsThenReturnsFailure()
    {
        await base.GivenImportCsvWhenDataIntegrityFailsThenReturnsFailure();
    }

    [Fact]
    public override async Task GivenImportExcelWhenDataIsValidThenImportsData()
    {
        await base.GivenImportExcelWhenDataIsValidThenImportsData();
    }

    [Fact]
    public override async Task GivenImportExcelWhenFileParseFailsThenReturnsFailure()
    {
        await base.GivenImportExcelWhenFileParseFailsThenReturnsFailure();
    }

    [Fact]
    public override async Task GivenImportExcelWhenExcelMapNotFoundThenReturnsFailure()
    {
        await base.GivenImportExcelWhenExcelMapNotFoundThenReturnsFailure();
    }

    [Fact]
    public override async Task GivenImportExcelWhenValidationFailsThenReturnsFailure()
    {
        await base.GivenImportExcelWhenValidationFailsThenReturnsFailure();
    }

    [Fact]
    public override async Task GivenImportExcelWhenDataIntegrityFailsThenReturnsFailure()
    {
        await base.GivenImportExcelWhenDataIntegrityFailsThenReturnsFailure();
    }
}
