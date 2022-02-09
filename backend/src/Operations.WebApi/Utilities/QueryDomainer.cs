namespace BoringFinances.Operations.WebApi.Utilities;

public class QueryDomainer : IQueryDomainer
{
    public IQueryable<T> PaginateQueryFrom<T>(IQueryable<T> query, IPagination pagination)
    {
        int offset = pagination.PageSize * (pagination.PageNumber - 1);
        var paginatedQuery = query.Skip(offset).Take(pagination.PageSize);
        return paginatedQuery;
    }

    public IQueryable<T> LimitQueryFrom<T>(IQueryable<T> query, ILimitation limitation)
    {
        var queryWithOffsetOrNot = limitation.Offset is not null
            ? query.Skip(limitation.Offset.Value)
            : query;

        var queryWithLimitOrNot = limitation.Limit is not null
            ? queryWithOffsetOrNot.Take(limitation.Limit.Value)
            : queryWithOffsetOrNot;

        return queryWithLimitOrNot;
    }
}
