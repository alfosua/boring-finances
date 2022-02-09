using BoringFinances.Operations.WebApi.Utilities;

namespace BoringFinances.Operations.WebApi.Accounts;

public interface IAccountLister
{
    Task<CountedEnumerable<AccountDto>> ListManyByFilteringAsync(AccountLimitedFilter filter);

    Task<PagedEnumerable<AccountDto>> PageManyByFilteringAsync(AccountPagedFilter filter);
}
