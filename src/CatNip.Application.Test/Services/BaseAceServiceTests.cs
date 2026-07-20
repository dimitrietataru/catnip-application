using CatNip.Application.Services;
using CatNip.Application.Test.Services.Abstractions;
using CatNip.Domain.ImportExport.Csv;
using CatNip.Domain.Models.Interfaces;
using CatNip.Domain.Query.Filtering;
using CatNip.Domain.Repositories;

namespace CatNip.Application.Test.Services;

public abstract partial class BaseAceServiceTests<TService, TRepository, TModel, TId, TFiltering, TExchange>
    : BaseCrudServiceTests<TService, TRepository, TModel, TId>, IAceServiceTests
    where TService : AceService<TRepository, TModel, TId, TFiltering, TExchange>
    where TRepository : class, IAceRepository<TModel, TId, TFiltering, TExchange>
    where TModel : IModel<TId>
    where TId : IEquatable<TId>
    where TFiltering : IFilteringRequest
    where TExchange : ICsvMappable
{
    protected abstract Mock<ICsvConverter> CsvConverterMock { get; }
}
