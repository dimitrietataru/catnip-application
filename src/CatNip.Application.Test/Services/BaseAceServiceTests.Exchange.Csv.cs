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
    public virtual async Task GivenImportCsvWhenDataIsValidThenImportsData()
    {
        // Arrange
        using var request = new ImportRequest(new MemoryStream(0), "foo.csv");
        ArrangeImportCsvOnSuccess(request);

        // Act
        var response = await Service.ImportCsvAsync(request, CancellationToken.None);

        // Assert
        AssertImportCsvOnSuccess(response);
    }

    public virtual async Task GivenImportCsvWhenFileParseFailsThenReturnsFailure()
    {
        // Arrange
        using var request = new ImportRequest(new MemoryStream(0), "foo.csv");
        ArrangeImportCsvOnFailureFileParse(request);

        // Act
        var response = await Service.ImportCsvAsync(request, CancellationToken.None);

        // Assert
        AssertImportCsvOnFailureFileParse(response);
    }

    public virtual async Task GivenImportCsvWhenCsvMapNotFoundThenReturnsFailure()
    {
        // Arrange
        using var request = new ImportRequest(new MemoryStream(0), "foo.csv");
        ArrangeImportCsvOnFailureCsvMapNotFound(request);

        // Act
        var response = await Service.ImportCsvAsync(request, CancellationToken.None);

        // Assert
        AssertImportCsvOnFailureCsvMapNotFound(response);
    }

    public virtual async Task GivenImportCsvWhenValidationFailsThenReturnsFailure()
    {
        // Arrange
        using var request = new ImportRequest(new MemoryStream(0), "foo.csv");
        ArrangeImportCsvOnFailureValidation(request);

        // Act
        var response = await Service.ImportCsvAsync(request, CancellationToken.None);

        // Assert
        AssertImportCsvOnFailureValidation(response);
    }

    public virtual async Task GivenImportCsvWhenDataIntegrityFailsThenReturnsFailure()
    {
        // Arrange
        using var request = new ImportRequest(new MemoryStream(0), "foo.csv");
        ArrangeImportCsvOnFailureDataIntegrity(request);

        // Act
        var response = await Service.ImportCsvAsync(request, CancellationToken.None);

        // Assert
        AssertImportCsvOnFailureDataIntegrity(response);
    }

    protected virtual void ArrangeImportCsvOnSuccess(ImportRequest request)
    {
        var response = ImportResponse.Success(totalRows: 2, createdRecords: 1, updatedRecords: 1);

        CsvConverterMock
            .Setup(_ => _.ReadAsync<TExchange>(request.Stream, It.IsAny<CancellationToken>()))
            .ReturnsAsync([])
            .Verifiable();
        RepositoryMock
            .Setup(_ => _.ImportAsync(It.IsAny<ICollection<TExchange>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
    }

    protected virtual void AssertImportCsvOnSuccess(ImportResponse response)
    {
        response.Should().NotBeNull().And.BeOfType<ImportResponse>();
        response.IsSuccessful.Should().BeTrue();

        CsvConverterMock.Verify(
            _ => _.ReadAsync<TExchange>(It.IsAny<Stream>(), It.IsAny<CancellationToken>()), Times.Once);
        CsvConverterMock.VerifyNoOtherCalls();
        CsvConverterMock.VerifyAll();

        RepositoryMock.Verify(
            _ => _.ImportAsync(It.IsAny<ICollection<TExchange>>(), It.IsAny<CancellationToken>()), Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }

    protected virtual void ArrangeImportCsvOnFailureFileParse(ImportRequest request)
    {
        CsvConverterMock
            .Setup(_ => _.ReadAsync<TExchange>(request.Stream, It.IsAny<CancellationToken>()))
            .Throws<Exception>()
            .Verifiable();
    }

    protected virtual void AssertImportCsvOnFailureFileParse(ImportResponse importResponse)
    {
        importResponse.Should().NotBeNull().And.BeOfType<ImportResponse>();
        importResponse.IsSuccessful.Should().BeFalse();
        importResponse.Errors.Should().NotBeEmpty().And.HaveCount(1);
        importResponse.Errors.First().Should().NotBeNull().And.BeOfType<ImportParseError>();

        CsvConverterMock.Verify(
            _ => _.ReadAsync<TExchange>(It.IsAny<Stream>(), It.IsAny<CancellationToken>()), Times.Once);
        CsvConverterMock.VerifyNoOtherCalls();
        CsvConverterMock.VerifyAll();

        RepositoryMock.Verify(
            _ => _.ImportAsync(It.IsAny<ICollection<TExchange>>(), It.IsAny<CancellationToken>()), Times.Never);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }

    protected virtual void ArrangeImportCsvOnFailureCsvMapNotFound(ImportRequest request)
    {
        CsvConverterMock
            .Setup(_ => _.ReadAsync<TExchange>(request.Stream, It.IsAny<CancellationToken>()))
            .Throws<CsvMappingNotFoundException>()
            .Verifiable();
    }

    protected virtual void AssertImportCsvOnFailureCsvMapNotFound(ImportResponse importResponse)
    {
        importResponse.Should().NotBeNull().And.BeOfType<ImportResponse>();
        importResponse.IsSuccessful.Should().BeFalse();
        importResponse.Errors.Should().NotBeEmpty().And.HaveCount(1);
        importResponse.Errors.First().Should().NotBeNull().And.BeOfType<ImportParseError>();

        CsvConverterMock.Verify(
            _ => _.ReadAsync<TExchange>(It.IsAny<Stream>(), It.IsAny<CancellationToken>()), Times.Once);
        CsvConverterMock.VerifyNoOtherCalls();
        CsvConverterMock.VerifyAll();

        RepositoryMock.Verify(
            _ => _.ImportAsync(It.IsAny<ICollection<TExchange>>(), It.IsAny<CancellationToken>()), Times.Never);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }

    protected virtual void ArrangeImportCsvOnFailureValidation(ImportRequest request)
    {
        var response = ImportResponse.Failure(new ImportValidationError("Validation Error"));

        CsvConverterMock
            .Setup(_ => _.ReadAsync<TExchange>(request.Stream, It.IsAny<CancellationToken>()))
            .ReturnsAsync([])
            .Verifiable();
        RepositoryMock
            .Setup(_ => _.ImportAsync(It.IsAny<ICollection<TExchange>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
    }

    protected virtual void AssertImportCsvOnFailureValidation(ImportResponse response)
    {
        response.Should().NotBeNull().And.BeOfType<ImportResponse>();
        response.IsSuccessful.Should().BeFalse();
        response.Errors.Should().NotBeEmpty().And.HaveCountGreaterThanOrEqualTo(1);
        response.Errors.First().Should().NotBeNull().And.BeOfType<ImportValidationError>();

        CsvConverterMock.Verify(
            _ => _.ReadAsync<TExchange>(It.IsAny<Stream>(), It.IsAny<CancellationToken>()), Times.Once);
        CsvConverterMock.VerifyNoOtherCalls();
        CsvConverterMock.VerifyAll();

        RepositoryMock.Verify(
            _ => _.ImportAsync(It.IsAny<ICollection<TExchange>>(), It.IsAny<CancellationToken>()), Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }

    protected virtual void ArrangeImportCsvOnFailureDataIntegrity(ImportRequest request)
    {
        var response = ImportResponse.Failure(new ImportDataIntegrityError("Data Integrity Error"));

        CsvConverterMock
            .Setup(_ => _.ReadAsync<TExchange>(request.Stream, It.IsAny<CancellationToken>()))
            .ReturnsAsync([])
            .Verifiable();
        RepositoryMock
            .Setup(_ => _.ImportAsync(It.IsAny<ICollection<TExchange>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
    }

    protected virtual void AssertImportCsvOnFailureDataIntegrity(ImportResponse response)
    {
        response.Should().NotBeNull().And.BeOfType<ImportResponse>();
        response.IsSuccessful.Should().BeFalse();
        response.Errors.Should().NotBeEmpty().And.HaveCountGreaterThanOrEqualTo(1);
        response.Errors.First().Should().NotBeNull().And.BeOfType<ImportDataIntegrityError>();

        CsvConverterMock.Verify(
            _ => _.ReadAsync<TExchange>(It.IsAny<Stream>(), It.IsAny<CancellationToken>()), Times.Once);
        CsvConverterMock.VerifyNoOtherCalls();
        CsvConverterMock.VerifyAll();

        RepositoryMock.Verify(
            _ => _.ImportAsync(It.IsAny<ICollection<TExchange>>(), It.IsAny<CancellationToken>()), Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }
}
