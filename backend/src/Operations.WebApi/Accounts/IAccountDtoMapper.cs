using BoringFinances.Operations.Data.Accounts;
using System.Linq.Expressions;

namespace BoringFinances.Operations.WebApi.Accounts;

public interface IAccountDtoMapper
{
    Expression<Func<Account, AccountDto>> SelectExpression { get; }

    AccountDto CreateAccountDtoFrom(Account account);
}
