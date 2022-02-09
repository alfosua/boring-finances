using BoringFinances.Operations.Data;
using BoringFinances.Operations.Data.Operations;
using BoringFinances.Operations.WebApi.Utilities;
using Microsoft.EntityFrameworkCore;

namespace BoringFinances.Operations.WebApi.Operations;

public class OperationLister : IOperationLister
{
    private readonly OperationDbContext _dbContext;
    private readonly IOperationDtoMapper _operationDtoMapper;
    private readonly IQueryDomainer _queryDomainer;

    public OperationLister(OperationDbContext dbContext, IOperationDtoMapper operationDtoMapper, IQueryDomainer queryDomainer)
    {
        _dbContext = dbContext;
        _operationDtoMapper = operationDtoMapper;
        _queryDomainer = queryDomainer;
    }

    public async Task<CountedEnumerable<OperationDto>> ListManyByFilteringAsync(OperationLimitedFilter filter)
    {
        var items = await QueryItemsFromLimitedFilterAsync(filter);
        var filteredCount = await QueryCountFromFilterAsync(filter);
        var totalCount = await QueryTotalCountAsync();

        return new()
        {
            Items = items,
            Limitation = filter,
            FilteredCount = filteredCount,
            TotalCount = totalCount,
        };
    }

    public async Task<PagedEnumerable<OperationDto>> PageManyByFilteringAsync(OperationPagedFilter filter)
    {
        var items = await QueryItemsFromPagedFilterAsync(filter);
        var filteredCount = await QueryCountFromFilterAsync(filter);
        var totalCount = await QueryTotalCountAsync();

        return new()
        {
            Items = items,
            Pagination = filter,
            FilteredCount = filteredCount,
            TotalCount = totalCount,
        };
    }

    private async Task<List<OperationDto>> QueryItemsFromLimitedFilterAsync(OperationLimitedFilter filter)
    {
        var baseQuery = _dbContext.Operations.AsQueryable();
        var filtering = FilterQueryFrom(baseQuery, filter);
        var limitation = _queryDomainer.LimitQueryFrom(filtering, filter);
        var projection = limitation.Select(_operationDtoMapper.MostRecentVersionMappingExpression);
        var result = await projection.ToListAsync();
        return result;
    }

    private async Task<List<OperationDto>> QueryItemsFromPagedFilterAsync(OperationPagedFilter filter)
    {
        var baseQuery = _dbContext.Operations.AsQueryable();
        var filtering = FilterQueryFrom(baseQuery, filter);
        var pagination = _queryDomainer.PaginateQueryFrom(filtering, filter);
        var projection = pagination.Select(_operationDtoMapper.MostRecentVersionMappingExpression);
        var result = await projection.ToListAsync();
        return result;
    }

    private async Task<int> QueryCountFromFilterAsync(IOperationFilter filter)
    {
        var baseQuery = _dbContext.Operations.AsQueryable();
        var filtering = FilterQueryFrom(baseQuery, filter);
        var count = await filtering.CountAsync();
        return count;
    }

    private async Task<int> QueryTotalCountAsync()
    {
        var baseQuery = _dbContext.Operations.AsQueryable();
        var defaultFiltering = baseQuery.Where(x => !x.Deleted.HasValue);
        var count = await defaultFiltering.CountAsync();
        return count;
    }

    private IQueryable<Operation> FilterQueryFrom(IQueryable<Operation> query, IOperationFilter filter)
    {
        var loweredText = filter.Text?.ToLowerInvariant();
        var loweredTags = filter.Tags?
            .Select(x => x.ToLowerInvariant())
            .ToList();

        var defaultFiltering = query.Where(x => !x.Deleted.HasValue);

        var idsFiltering = (filter.Ids?.Any() ?? false)
            ? defaultFiltering.Where(x => filter.Ids.Contains(x.OperationId))
            : defaultFiltering;

        var tagsFiltering = (filter.Tags?.Any() ?? false)
            ? query.Where(x => x.Tags.Any(x => loweredTags!.Contains(x.Name)))
            : idsFiltering;

        var textFiltering = loweredText is not null && string.IsNullOrEmpty(loweredText)
            ? query.Where(x => x.Tags.Any(x => loweredText.Contains(x.Name))
                || x.Notes.Any(x => loweredText.Contains(x.Text.ToLowerInvariant()))
                || x.Entries.Any(x => x.Notes.Any(x => loweredText.Contains(x.Text.ToLowerInvariant()))))
            : tagsFiltering;

        return textFiltering;
    }
}
