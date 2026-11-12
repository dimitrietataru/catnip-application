using CatNip.Application.Services;
using CatNip.Domain.Exceptions;
using CatNip.Domain.ImportExport;
using CatNip.Domain.ImportExport.Csv;
using CatNip.Domain.ImportExport.Excel;
using CatNip.Domain.Models.Interfaces;
using CatNip.Domain.Query;
using CatNip.Domain.Query.Filtering;
using CatNip.Domain.Repositories;

namespace CatNip.Application.Test.Services;

public abstract class BaseAceServiceTests<TService, TRepository, TModel, TId, TModelRoot, TFiltering, TExchange>
    : BaseAceServiceTests<TService, TRepository, TModel, TId, TId, TModelRoot, TFiltering, TExchange>
    where TService : AceService<TRepository, TModel, TId, TModelRoot, TFiltering, TExchange>
    where TRepository : class, IAceRepository<TModel, TId, TModelRoot, TFiltering, TExchange>
    where TModel : IAceModel<TId>
    where TId : IEquatable<TId>
    where TModelRoot : ICrudModel<TId>
    where TFiltering : IFilteringRequest
    where TExchange : ICsvMappable, IExcelMappable
{
}

public abstract class BaseAceServiceTests<TService, TRepository, TModel, TId, TTraceId, TModelRoot, TFiltering, TExchange>
    : BaseCrudServiceTests<TService, TRepository, TModel, TId>
    where TService : AceService<TRepository, TModel, TId, TTraceId, TModelRoot, TFiltering, TExchange>
    where TRepository : class, IAceRepository<TModel, TId, TTraceId, TModelRoot, TFiltering, TExchange>
    where TModel : IAceModel<TId, TTraceId>
    where TId : IEquatable<TId>
    where TTraceId : IEquatable<TTraceId>
    where TModelRoot : ICrudModel<TId>
    where TFiltering : IFilteringRequest
    where TExchange : ICsvMappable, IExcelMappable
{
    protected abstract Mock<ICsvConverter> CsvConverterMock { get; }
    protected abstract Mock<IExcelConverter> ExcelConverterMock { get; }

    public virtual async Task GivenGetByFilterWhenDataExistsThenReturnsData()
    {
        // Arrange
        RepositoryMock
            .Setup(_ => _.GetAsync(It.IsAny<QueryRequest<TFiltering>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new QueryResponse<TModelRoot>())
            .Verifiable();

        // Act
        var result = await Service.GetAsync(It.IsAny<QueryRequest<TFiltering>>(), It.IsAny<CancellationToken>());

        // Assert
        result.Should().NotBeNull().And.BeOfType<QueryResponse<TModelRoot>>();
        RepositoryMock.Verify(_ => _.GetAsync(It.IsAny<QueryRequest<TFiltering>>(), It.IsAny<CancellationToken>()), Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }

    public virtual async Task GivenCountByFilterWhenDataExistsThenReturnsCount()
    {
        // Arrange
        RepositoryMock
            .Setup(_ => _.CountAsync(It.IsAny<TFiltering>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(0)
            .Verifiable();

        // Act
        int count = await Service.CountAsync(It.IsAny<TFiltering>(), It.IsAny<CancellationToken>());

        // Assert
        count.Should().BeGreaterThanOrEqualTo(0);
        RepositoryMock.Verify(_ => _.CountAsync(It.IsAny<TFiltering>(), It.IsAny<CancellationToken>()), Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }

    public virtual async Task GivenExistsByFilterWhenDataExistsThenReturnsTrue()
    {
        // Arrange
        RepositoryMock
            .Setup(_ => _.ExistsAsync(It.IsAny<TFiltering>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true)
            .Verifiable();

        // Act
        bool exists = await Service.ExistsAsync(It.IsAny<TFiltering>(), It.IsAny<CancellationToken>());

        // Assert
        exists.Should().BeTrue();
        RepositoryMock.Verify(_ => _.ExistsAsync(It.IsAny<TFiltering>(), It.IsAny<CancellationToken>()), Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }

    public virtual async Task GivenExistsByFilterWhenDataNotFoundThenReturnsFalse()
    {
        // Arrange
        RepositoryMock
            .Setup(_ => _.ExistsAsync(It.IsAny<TFiltering>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false)
            .Verifiable();

        // Act
        bool exists = await Service.ExistsAsync(It.IsAny<TFiltering>(), It.IsAny<CancellationToken>());

        // Assert
        exists.Should().BeFalse();
        RepositoryMock.Verify(_ => _.ExistsAsync(It.IsAny<TFiltering>(), It.IsAny<CancellationToken>()), Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }

    public virtual async Task GivenImportCsvWhenDataIsValidThenImportsData()
    {
        // Arrange
        using var request = new ImportRequest(new MemoryStream(0), "foo.csv");
        CsvConverterMock
            .Setup(_ => _.ReadAsync<TExchange>(request.Stream, It.IsAny<CancellationToken>()))
            .ReturnsAsync([])
            .Verifiable();
        RepositoryMock
            .Setup(_ => _.ImportAsync(It.IsAny<ICollection<TExchange>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ImportResponse.Success(totalRows: 2, createdRecords: 1, updatedRecords: 1));

        // Act
        var response = await Service.ImportCsvAsync(request, CancellationToken.None);

        // Assert
        response.Should().NotBeNull().And.BeOfType<ImportResponse>();
        response.IsSuccessful.Should().BeTrue();
        CsvConverterMock.Verify(_ => _.ReadAsync<TExchange>(It.IsAny<Stream>(), It.IsAny<CancellationToken>()), Times.Once);
        CsvConverterMock.VerifyNoOtherCalls();
        CsvConverterMock.VerifyAll();
        RepositoryMock.Verify(_ => _.ImportAsync(It.IsAny<ICollection<TExchange>>(), It.IsAny<CancellationToken>()), Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }

    public virtual async Task GivenImportCsvWhenFileParseFailsThenReturnsFailure()
    {
        // Arrange
        using var request = new ImportRequest(new MemoryStream(0), "foo.csv");
        CsvConverterMock
            .Setup(_ => _.ReadAsync<TExchange>(request.Stream, It.IsAny<CancellationToken>()))
            .Throws<Exception>()
            .Verifiable();

        // Act
        var response = await Service.ImportCsvAsync(request, CancellationToken.None);

        // Assert
        response.Should().NotBeNull().And.BeOfType<ImportResponse>();
        response.IsSuccessful.Should().BeFalse();
        response.Errors.Should().NotBeEmpty().And.HaveCount(1);
        response.Errors.First().Should().NotBeNull().And.BeOfType<ImportParseError>();
        CsvConverterMock.Verify(_ => _.ReadAsync<TExchange>(It.IsAny<Stream>(), It.IsAny<CancellationToken>()), Times.Once);
        CsvConverterMock.VerifyNoOtherCalls();
        CsvConverterMock.VerifyAll();
        RepositoryMock.Verify(_ => _.ImportAsync(It.IsAny<ICollection<TExchange>>(), It.IsAny<CancellationToken>()), Times.Never);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }

    public virtual async Task GivenImportCsvWhenCsvMapNotFoundThenReturnsFailure()
    {
        // Arrange
        using var request = new ImportRequest(new MemoryStream(0), "foo.csv");
        CsvConverterMock
            .Setup(_ => _.ReadAsync<TExchange>(request.Stream, It.IsAny<CancellationToken>()))
            .Throws<CsvMappingNotFoundException>()
            .Verifiable();

        // Act
        var response = await Service.ImportCsvAsync(request, CancellationToken.None);

        // Assert
        response.Should().NotBeNull().And.BeOfType<ImportResponse>();
        response.IsSuccessful.Should().BeFalse();
        response.Errors.Should().NotBeEmpty().And.HaveCount(1);
        response.Errors.First().Should().NotBeNull().And.BeOfType<ImportParseError>();
        CsvConverterMock.Verify(_ => _.ReadAsync<TExchange>(It.IsAny<Stream>(), It.IsAny<CancellationToken>()), Times.Once);
        CsvConverterMock.VerifyNoOtherCalls();
        CsvConverterMock.VerifyAll();
        RepositoryMock.Verify(_ => _.ImportAsync(It.IsAny<ICollection<TExchange>>(), It.IsAny<CancellationToken>()), Times.Never);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }

    public virtual async Task GivenImportCsvWhenValidationFailsThenReturnsFailure()
    {
        // Arrange
        using var request = new ImportRequest(new MemoryStream(0), "foo.csv");
        CsvConverterMock
            .Setup(_ => _.ReadAsync<TExchange>(request.Stream, It.IsAny<CancellationToken>()))
            .ReturnsAsync([])
            .Verifiable();
        RepositoryMock
            .Setup(_ => _.ImportAsync(It.IsAny<ICollection<TExchange>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ImportResponse.Failure(new ImportValidationError("Validation Error")));

        // Act
        var response = await Service.ImportCsvAsync(request, CancellationToken.None);

        // Assert
        response.Should().NotBeNull().And.BeOfType<ImportResponse>();
        response.IsSuccessful.Should().BeFalse();
        response.Errors.Should().NotBeEmpty().And.HaveCountGreaterThanOrEqualTo(1);
        response.Errors.First().Should().NotBeNull().And.BeOfType<ImportValidationError>();
        CsvConverterMock.Verify(_ => _.ReadAsync<TExchange>(It.IsAny<Stream>(), It.IsAny<CancellationToken>()), Times.Once);
        CsvConverterMock.VerifyNoOtherCalls();
        CsvConverterMock.VerifyAll();
        RepositoryMock.Verify(_ => _.ImportAsync(It.IsAny<ICollection<TExchange>>(), It.IsAny<CancellationToken>()), Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }

    public virtual async Task GivenImportCsvWhenDataIntegrityFailsThenReturnsFailure()
    {
        // Arrange
        using var request = new ImportRequest(new MemoryStream(0), "foo.csv");
        CsvConverterMock
            .Setup(_ => _.ReadAsync<TExchange>(request.Stream, It.IsAny<CancellationToken>()))
            .ReturnsAsync([])
            .Verifiable();
        RepositoryMock
            .Setup(_ => _.ImportAsync(It.IsAny<ICollection<TExchange>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ImportResponse.Failure(new ImportDataIntegrityError("Data Integrity Error")));

        // Act
        var response = await Service.ImportCsvAsync(request, CancellationToken.None);

        // Assert
        response.Should().NotBeNull().And.BeOfType<ImportResponse>();
        response.IsSuccessful.Should().BeFalse();
        response.Errors.Should().NotBeEmpty().And.HaveCountGreaterThanOrEqualTo(1);
        response.Errors.First().Should().NotBeNull().And.BeOfType<ImportDataIntegrityError>();
        CsvConverterMock.Verify(_ => _.ReadAsync<TExchange>(It.IsAny<Stream>(), It.IsAny<CancellationToken>()), Times.Once);
        CsvConverterMock.VerifyNoOtherCalls();
        CsvConverterMock.VerifyAll();
        RepositoryMock.Verify(_ => _.ImportAsync(It.IsAny<ICollection<TExchange>>(), It.IsAny<CancellationToken>()), Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }

    public virtual async Task GivenImportExcelWhenDataIsValidThenImportsData()
    {
        // Arrange
        using var request = new ImportRequest(new MemoryStream(0), "foo.xlsx");
        ExcelConverterMock
            .Setup(_ => _.ReadAsync<TExchange>(request.Stream, It.IsAny<CancellationToken>()))
            .ReturnsAsync([])
            .Verifiable();
        RepositoryMock
            .Setup(_ => _.ImportAsync(It.IsAny<ICollection<TExchange>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ImportResponse.Success(totalRows: 2, createdRecords: 1, updatedRecords: 1));

        // Act
        var response = await Service.ImportExcelAsync(request, CancellationToken.None);

        // Assert
        response.Should().NotBeNull().And.BeOfType<ImportResponse>();
        response.IsSuccessful.Should().BeTrue();
        ExcelConverterMock.Verify(_ => _.ReadAsync<TExchange>(It.IsAny<Stream>(), It.IsAny<CancellationToken>()), Times.Once);
        ExcelConverterMock.VerifyNoOtherCalls();
        ExcelConverterMock.VerifyAll();
        RepositoryMock.Verify(_ => _.ImportAsync(It.IsAny<ICollection<TExchange>>(), It.IsAny<CancellationToken>()), Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }

    public virtual async Task GivenImportExcelWhenFileParseFailsThenReturnsFailure()
    {
        // Arrange
        using var request = new ImportRequest(new MemoryStream(0), "foo.xlsx");
        ExcelConverterMock
            .Setup(_ => _.ReadAsync<TExchange>(request.Stream, It.IsAny<CancellationToken>()))
            .Throws<Exception>()
            .Verifiable();

        // Act
        var response = await Service.ImportExcelAsync(request, CancellationToken.None);

        // Assert
        response.Should().NotBeNull().And.BeOfType<ImportResponse>();
        response.IsSuccessful.Should().BeFalse();
        response.Errors.Should().NotBeEmpty().And.HaveCount(1);
        response.Errors.First().Should().NotBeNull().And.BeOfType<ImportParseError>();
        ExcelConverterMock.Verify(_ => _.ReadAsync<TExchange>(It.IsAny<Stream>(), It.IsAny<CancellationToken>()), Times.Once);
        ExcelConverterMock.VerifyNoOtherCalls();
        ExcelConverterMock.VerifyAll();
        RepositoryMock.Verify(_ => _.ImportAsync(It.IsAny<ICollection<TExchange>>(), It.IsAny<CancellationToken>()), Times.Never);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }

    public virtual async Task GivenImportExcelWhenExcelMapNotFoundThenReturnsFailure()
    {
        // Arrange
        using var request = new ImportRequest(new MemoryStream(0), "foo.xlsx");
        ExcelConverterMock
            .Setup(_ => _.ReadAsync<TExchange>(request.Stream, It.IsAny<CancellationToken>()))
            .Throws<ExcelMappingNotFoundException>()
            .Verifiable();

        // Act
        var response = await Service.ImportExcelAsync(request, CancellationToken.None);

        // Assert
        response.Should().NotBeNull().And.BeOfType<ImportResponse>();
        response.IsSuccessful.Should().BeFalse();
        response.Errors.Should().NotBeEmpty().And.HaveCount(1);
        response.Errors.First().Should().NotBeNull().And.BeOfType<ImportParseError>();
        ExcelConverterMock.Verify(_ => _.ReadAsync<TExchange>(It.IsAny<Stream>(), It.IsAny<CancellationToken>()), Times.Once);
        ExcelConverterMock.VerifyNoOtherCalls();
        ExcelConverterMock.VerifyAll();
        RepositoryMock.Verify(_ => _.ImportAsync(It.IsAny<ICollection<TExchange>>(), It.IsAny<CancellationToken>()), Times.Never);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }

    public virtual async Task GivenImportExcelWhenValidationFailsThenReturnsFailure()
    {
        // Arrange
        using var request = new ImportRequest(new MemoryStream(0), "foo.xlsx");
        ExcelConverterMock
            .Setup(_ => _.ReadAsync<TExchange>(request.Stream, It.IsAny<CancellationToken>()))
            .ReturnsAsync([])
            .Verifiable();
        RepositoryMock
            .Setup(_ => _.ImportAsync(It.IsAny<ICollection<TExchange>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ImportResponse.Failure(new ImportValidationError("Validation Error")));

        // Act
        var response = await Service.ImportExcelAsync(request, CancellationToken.None);

        // Assert
        response.Should().NotBeNull().And.BeOfType<ImportResponse>();
        response.IsSuccessful.Should().BeFalse();
        response.Errors.Should().NotBeEmpty().And.HaveCountGreaterThanOrEqualTo(1);
        response.Errors.First().Should().NotBeNull().And.BeOfType<ImportValidationError>();
        ExcelConverterMock.Verify(_ => _.ReadAsync<TExchange>(It.IsAny<Stream>(), It.IsAny<CancellationToken>()), Times.Once);
        ExcelConverterMock.VerifyNoOtherCalls();
        ExcelConverterMock.VerifyAll();
        RepositoryMock.Verify(_ => _.ImportAsync(It.IsAny<ICollection<TExchange>>(), It.IsAny<CancellationToken>()), Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }

    public virtual async Task GivenImportExcelWhenDataIntegrityFailsThenReturnsFailure()
    {
        // Arrange
        using var request = new ImportRequest(new MemoryStream(0), "foo.xlsx");
        ExcelConverterMock
            .Setup(_ => _.ReadAsync<TExchange>(request.Stream, It.IsAny<CancellationToken>()))
            .ReturnsAsync([])
            .Verifiable();
        RepositoryMock
            .Setup(_ => _.ImportAsync(It.IsAny<ICollection<TExchange>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ImportResponse.Failure(new ImportDataIntegrityError("Data Integrity Error")));

        // Act
        var response = await Service.ImportExcelAsync(request, CancellationToken.None);

        // Assert
        response.Should().NotBeNull().And.BeOfType<ImportResponse>();
        response.IsSuccessful.Should().BeFalse();
        response.Errors.Should().NotBeEmpty().And.HaveCountGreaterThanOrEqualTo(1);
        response.Errors.First().Should().NotBeNull().And.BeOfType<ImportDataIntegrityError>();
        ExcelConverterMock.Verify(_ => _.ReadAsync<TExchange>(It.IsAny<Stream>(), It.IsAny<CancellationToken>()), Times.Once);
        ExcelConverterMock.VerifyNoOtherCalls();
        ExcelConverterMock.VerifyAll();
        RepositoryMock.Verify(_ => _.ImportAsync(It.IsAny<ICollection<TExchange>>(), It.IsAny<CancellationToken>()), Times.Once);
        RepositoryMock.VerifyNoOtherCalls();
        RepositoryMock.VerifyAll();
    }
}
