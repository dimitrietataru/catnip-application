using CatNip.Application.Services;
using CatNip.Domain.Exceptions;
using CatNip.Domain.ImportExport;
using CatNip.Domain.ImportExport.Csv;
using CatNip.Domain.ImportExport.Excel;
using CatNip.Domain.Models.Interfaces;
using CatNip.Domain.Query.Filtering;
using CatNip.Domain.Repositories;

namespace CatNip.Application.Test.Services;

public abstract partial class BaseAceServiceTests<TService, TRepository, TModel, TId, TFiltering, TExchange>
    : BaseCrudServiceTests<TService, TRepository, TModel, TId>
    where TService : AceService<TRepository, TModel, TId, TFiltering, TExchange>
    where TRepository : class, IAceRepository<TModel, TId, TFiltering, TExchange>
    where TModel : IModel<TId>
    where TId : IEquatable<TId>
    where TFiltering : IFilteringRequest
    where TExchange : ICsvMappable, IExcelMappable
{
    public virtual async Task GivenImportExcelWhenDataIsValidThenImportsData()
    {
        // Arrange
        using var request = new ImportRequest(new MemoryStream(0), "foo.xlsx");
        ArrangeImportExcelOnSuccess(request);

        // Act
        var response = await Service.ImportExcelAsync(request, CancellationToken.None);

        // Assert
        AssertImportExcelOnSuccess(response);
    }

    public virtual async Task GivenImportExcelWhenFileParseFailsThenReturnsFailure()
    {
        // Arrange
        using var request = new ImportRequest(new MemoryStream(0), "foo.xlsx");
        ArrangeImportExcelOnFailureFileParse(request);

        // Act
        var response = await Service.ImportExcelAsync(request, CancellationToken.None);

        // Assert
        AssertImportExcelOnFailureFileParse(response);
    }

    public virtual async Task GivenImportExcelWhenExcelMapNotFoundThenReturnsFailure()
    {
        // Arrange
        using var request = new ImportRequest(new MemoryStream(0), "foo.xlsx");
        ArrangeImportExcelOnFailureExcelMapNotFound(request);

        // Act
        var response = await Service.ImportExcelAsync(request, CancellationToken.None);

        // Assert
        AssertImportExcelOnFailureExcelMapNotFound(response);
    }

    public virtual async Task GivenImportExcelWhenValidationFailsThenReturnsFailure()
    {
        // Arrange
        using var request = new ImportRequest(new MemoryStream(0), "foo.xlsx");
        ArrangeImportExcelOnFailureValidation(request);

        // Act
        var response = await Service.ImportExcelAsync(request, CancellationToken.None);

        // Assert
        AssertImportExcelOnFailureValidation(response);
    }

    public virtual async Task GivenImportExcelWhenDataIntegrityFailsThenReturnsFailure()
    {
        // Arrange
        using var request = new ImportRequest(new MemoryStream(0), "foo.xlsx");
        ArrangeImportExcelOnFailureDataIntegrity(request);

        // Act
        var response = await Service.ImportExcelAsync(request, CancellationToken.None);

        // Assert
        AssertImportExcelOnFailureDataIntegrity(response);
    }

    protected virtual void ArrangeImportExcelOnSuccess(ImportRequest request)
    {
        var response = ImportResponse.Success(totalRows: 2, createdRecords: 1, updatedRecords: 1);

        ExcelConverterMock
            .Setup(_ => _.ReadAsync<TExchange>(request.Stream, It.IsAny<CancellationToken>()))
            .ReturnsAsync([])
            .Verifiable();
        RepositoryMock
            .Setup(_ => _.ImportAsync(It.IsAny<ICollection<TExchange>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
    }

    protected virtual void AssertImportExcelOnSuccess(ImportResponse response)
    {
        response.Should().NotBeNull().And.BeOfType<ImportResponse>();
        response.IsSuccessful.Should().BeTrue();

        ExcelConverterMock.Verify(
            _ => _.ReadAsync<TExchange>(It.IsAny<Stream>(), It.IsAny<CancellationToken>()), Times.Once);
        ExcelConverterMock.VerifyNoOtherCalls();
        ExcelConverterMock.VerifyAll();

        RepositoryMock.Verify(
            _ => _.ImportAsync(It.IsAny<ICollection<TExchange>>(), It.IsAny<CancellationToken>()), Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }

    protected virtual void ArrangeImportExcelOnFailureFileParse(ImportRequest request)
    {
        ExcelConverterMock
            .Setup(_ => _.ReadAsync<TExchange>(request.Stream, It.IsAny<CancellationToken>()))
            .Throws<Exception>()
            .Verifiable();
    }

    protected virtual void AssertImportExcelOnFailureFileParse(ImportResponse importResponse)
    {
        importResponse.Should().NotBeNull().And.BeOfType<ImportResponse>();
        importResponse.IsSuccessful.Should().BeFalse();
        importResponse.Errors.Should().NotBeEmpty().And.HaveCount(1);
        importResponse.Errors.First().Should().NotBeNull().And.BeOfType<ImportParseError>();

        ExcelConverterMock.Verify(
            _ => _.ReadAsync<TExchange>(It.IsAny<Stream>(), It.IsAny<CancellationToken>()), Times.Once);
        ExcelConverterMock.VerifyNoOtherCalls();
        ExcelConverterMock.VerifyAll();

        RepositoryMock.Verify(
            _ => _.ImportAsync(It.IsAny<ICollection<TExchange>>(), It.IsAny<CancellationToken>()), Times.Never);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }

    protected virtual void ArrangeImportExcelOnFailureExcelMapNotFound(ImportRequest request)
    {
        ExcelConverterMock
            .Setup(_ => _.ReadAsync<TExchange>(request.Stream, It.IsAny<CancellationToken>()))
            .Throws<ExcelMappingNotFoundException>()
            .Verifiable();
    }

    protected virtual void AssertImportExcelOnFailureExcelMapNotFound(ImportResponse importResponse)
    {
        importResponse.Should().NotBeNull().And.BeOfType<ImportResponse>();
        importResponse.IsSuccessful.Should().BeFalse();
        importResponse.Errors.Should().NotBeEmpty().And.HaveCount(1);
        importResponse.Errors.First().Should().NotBeNull().And.BeOfType<ImportParseError>();

        ExcelConverterMock.Verify(
            _ => _.ReadAsync<TExchange>(It.IsAny<Stream>(), It.IsAny<CancellationToken>()), Times.Once);
        ExcelConverterMock.VerifyNoOtherCalls();
        ExcelConverterMock.VerifyAll();

        RepositoryMock.Verify(
            _ => _.ImportAsync(It.IsAny<ICollection<TExchange>>(), It.IsAny<CancellationToken>()), Times.Never);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }

    protected virtual void ArrangeImportExcelOnFailureValidation(ImportRequest request)
    {
        var response = ImportResponse.Failure(new ImportValidationError("Validation Error"));

        ExcelConverterMock
            .Setup(_ => _.ReadAsync<TExchange>(request.Stream, It.IsAny<CancellationToken>()))
            .ReturnsAsync([])
            .Verifiable();
        RepositoryMock
            .Setup(_ => _.ImportAsync(It.IsAny<ICollection<TExchange>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
    }

    protected virtual void AssertImportExcelOnFailureValidation(ImportResponse response)
    {
        response.Should().NotBeNull().And.BeOfType<ImportResponse>();
        response.IsSuccessful.Should().BeFalse();
        response.Errors.Should().NotBeEmpty().And.HaveCountGreaterThanOrEqualTo(1);
        response.Errors.First().Should().NotBeNull().And.BeOfType<ImportValidationError>();

        ExcelConverterMock.Verify(
            _ => _.ReadAsync<TExchange>(It.IsAny<Stream>(), It.IsAny<CancellationToken>()), Times.Once);
        ExcelConverterMock.VerifyNoOtherCalls();
        ExcelConverterMock.VerifyAll();

        RepositoryMock.Verify(
            _ => _.ImportAsync(It.IsAny<ICollection<TExchange>>(), It.IsAny<CancellationToken>()), Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }

    protected virtual void ArrangeImportExcelOnFailureDataIntegrity(ImportRequest request)
    {
        var response = ImportResponse.Failure(new ImportDataIntegrityError("Data Integrity Error"));

        ExcelConverterMock
            .Setup(_ => _.ReadAsync<TExchange>(request.Stream, It.IsAny<CancellationToken>()))
            .ReturnsAsync([])
            .Verifiable();
        RepositoryMock
            .Setup(_ => _.ImportAsync(It.IsAny<ICollection<TExchange>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
    }

    protected virtual void AssertImportExcelOnFailureDataIntegrity(ImportResponse response)
    {
        response.Should().NotBeNull().And.BeOfType<ImportResponse>();
        response.IsSuccessful.Should().BeFalse();
        response.Errors.Should().NotBeEmpty().And.HaveCountGreaterThanOrEqualTo(1);
        response.Errors.First().Should().NotBeNull().And.BeOfType<ImportDataIntegrityError>();

        ExcelConverterMock.Verify(
            _ => _.ReadAsync<TExchange>(It.IsAny<Stream>(), It.IsAny<CancellationToken>()), Times.Once);
        ExcelConverterMock.VerifyNoOtherCalls();
        ExcelConverterMock.VerifyAll();

        RepositoryMock.Verify(
            _ => _.ImportAsync(It.IsAny<ICollection<TExchange>>(), It.IsAny<CancellationToken>()), Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }
}
