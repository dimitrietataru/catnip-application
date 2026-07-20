using CatNip.Domain.Exceptions;
using CatNip.Domain.ImportExport;
using CatNip.Domain.ImportExport.Csv;
using CatNip.Domain.Models.Interfaces;
using CatNip.Domain.Query;
using CatNip.Domain.Query.Filtering;
using CatNip.Domain.Repositories;
using CatNip.Domain.Services;

namespace CatNip.Application.Services;

public abstract class AceService<TRepository, TModel, TId, TFiltering, TExchange>
    : CrudService<TRepository, TModel, TId>, IAceService<TModel, TId, TFiltering, TExchange>
    where TRepository : IAceRepository<TModel, TId, TFiltering, TExchange>
    where TModel : IModel<TId>
    where TId : IEquatable<TId>
    where TFiltering : IFilteringRequest
    where TExchange : ICsvMappable
{
    protected virtual ICsvConverter CsvConverter { get; init; }

    protected AceService(TRepository repository, ICsvConverter csvConverter)
        : base(repository)
    {
        CsvConverter = csvConverter;
    }

    public virtual async Task<QueryResponse<TModel>> GetAsync(
        QueryRequest<TFiltering> request, CancellationToken cancellation = default)
    {
        var result = await Repository.GetAsync(request, cancellation);

        return result;
    }

    public virtual async Task<QueryResponse<TModelRoot>> GetAsync<TModelRoot>(
        QueryRequest<TFiltering> request, CancellationToken cancellation = default)
        where TModelRoot : IModel<TId>
    {
        var result = await Repository.GetAsync<TModelRoot>(request, cancellation);

        return result;
    }

    public virtual async Task<int> CountAsync(
        TFiltering filter, CancellationToken cancellation = default)
    {
        int count = await Repository.CountAsync(filter, cancellation);

        return count;
    }

    public virtual async Task<ImportResponse> ImportAsync(ImportRequest request, CancellationToken cancellation = default)
    {
        ICollection<TExchange> importRecords;

        try
        {
            importRecords = await CsvConverter.ReadAsync<TExchange>(request.Stream, cancellation);
        }
        catch (CsvMappingNotFoundException)
        {
            var failure = new ImportParseError($"Failed to read data from '{request.FileName}' file. Unmapped data structure.");
            return ImportResponse.Failure(failure);
        }
#pragma warning disable CA1031 // Do not catch general exception types
        catch (Exception)
#pragma warning restore CA1031 // Do not catch general exception types
        {
            var failure = new ImportParseError($"Failed to read data from '{request.FileName}' file.");
            return ImportResponse.Failure(failure);
        }

        return await Repository.ImportAsync(importRecords, cancellation);
    }
}
