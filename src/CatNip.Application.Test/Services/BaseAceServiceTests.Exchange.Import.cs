using CatNip.Application.Services;
using CatNip.Domain.Exceptions;
using CatNip.Domain.ImportExport;
using CatNip.Domain.ImportExport.Csv;
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
    where TExchange : ICsvMappable
{
    public virtual async Task GivenImportWhenDataIsValidThenImportsData()
    {
        // Arrange
        using var request = new ImportRequest(new MemoryStream(0), "foo.csv");
        ArrangeImportOnSuccess(request);

        // Act
        var response = await Service.ImportAsync(request, CancellationToken.None);

        // Assert
        AssertImportOnSuccess(response);
    }

    public virtual async Task GivenImportWhenFileParseFailsThenReturnsFailure()
    {
        // Arrange
        using var request = new ImportRequest(new MemoryStream(0), "foo.csv");
        ArrangeImportOnFailureFileParse(request);

        // Act
        var response = await Service.ImportAsync(request, CancellationToken.None);

        // Assert
        AssertImportOnFailureFileParse(response);
    }

    public virtual async Task GivenImportWhenCsvMapNotFoundThenReturnsFailure()
    {
        // Arrange
        using var request = new ImportRequest(new MemoryStream(0), "foo.csv");
        ArrangeImportOnFailureCsvMapNotFound(request);

        // Act
        var response = await Service.ImportAsync(request, CancellationToken.None);

        // Assert
        AssertImportOnFailureCsvMapNotFound(response);
    }

    public virtual async Task GivenImportWhenValidationFailsThenReturnsFailure()
    {
        // Arrange
        using var request = new ImportRequest(new MemoryStream(0), "foo.csv");
        ArrangeImportOnFailureValidation(request);

        // Act
        var response = await Service.ImportAsync(request, CancellationToken.None);

        // Assert
        AssertImportOnFailureValidation(response);
    }

    public virtual async Task GivenImportWhenDataIntegrityFailsThenReturnsFailure()
    {
        // Arrange
        using var request = new ImportRequest(new MemoryStream(0), "foo.csv");
        ArrangeImportOnFailureDataIntegrity(request);

        // Act
        var response = await Service.ImportAsync(request, CancellationToken.None);

        // Assert
        AssertImportOnFailureDataIntegrity(response);
    }

    protected virtual void ArrangeImportOnSuccess(ImportRequest request)
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

    protected virtual void AssertImportOnSuccess(ImportResponse response)
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

    protected virtual void ArrangeImportOnFailureFileParse(ImportRequest request)
    {
        CsvConverterMock
            .Setup(_ => _.ReadAsync<TExchange>(request.Stream, It.IsAny<CancellationToken>()))
            .Throws<Exception>()
            .Verifiable();
    }

    protected virtual void AssertImportOnFailureFileParse(ImportResponse importResponse)
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

    protected virtual void ArrangeImportOnFailureCsvMapNotFound(ImportRequest request)
    {
        CsvConverterMock
            .Setup(_ => _.ReadAsync<TExchange>(request.Stream, It.IsAny<CancellationToken>()))
            .Throws<CsvMappingNotFoundException>()
            .Verifiable();
    }

    protected virtual void AssertImportOnFailureCsvMapNotFound(ImportResponse importResponse)
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

    protected virtual void ArrangeImportOnFailureValidation(ImportRequest request)
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

    protected virtual void AssertImportOnFailureValidation(ImportResponse response)
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

    protected virtual void ArrangeImportOnFailureDataIntegrity(ImportRequest request)
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

    protected virtual void AssertImportOnFailureDataIntegrity(ImportResponse response)
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
