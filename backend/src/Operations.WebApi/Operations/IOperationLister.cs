using BoringFinances.Operations.WebApi.Utilities;

namespace BoringFinances.Operations.WebApi.Operations;

public interface IOperationLister
{
    Task<CountedEnumerable<OperationDto>> ListManyByFilteringAsync(OperationLimitedFilter filter);

    Task<PagedEnumerable<OperationDto>> PageManyByFilteringAsync(OperationPagedFilter filter);
}
