using BoringFinances.Operations.WebApi.Utilities;

namespace BoringFinances.Operations.WebApi.FinancialUnits;

public interface IFinancialUnitLister
{
    Task<CountedEnumerable<FinancialUnitDto>> ListManyByFilteringAsync(FinancialUnitLimitedFilter filter);

    Task<PagedEnumerable<FinancialUnitDto>> PageManyByFilteringAsync(FinancialUnitPagedFilter filter);
}
