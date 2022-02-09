namespace BoringFinances.Operations.WebApi.Utilities;

public interface IQueryDomainer
{
    IQueryable<T> LimitQueryFrom<T>(IQueryable<T> query, ILimitation limitation);

    IQueryable<T> PaginateQueryFrom<T>(IQueryable<T> query, IPagination pagination);
}
