using BoringFinances.Operations.Data.Accounts;
using System.Linq.Expressions;

namespace BoringFinances.Operations.WebApi.Accounts;

public class AccountDtoMapper : IAccountDtoMapper
{
    public Expression<Func<Account, AccountDto>> SelectExpression =>
        x => new AccountDto
        {
            AccountId = x.AccountId,
            Title = x.Title,
            AccountTypeId = x.AccountTypeId,
            Code = x.Code,
            Notes = x.Notes
                .Select(x => x.Text),
            Tags = x.Tags
                .Select(x => x.Name)
                .ToList(),
        };

    public AccountDto CreateAccountDtoFrom(Account account)
    {
        return new()
        {
            AccountId = account.AccountId,
            Title = account.Title,
            AccountTypeId = account.AccountTypeId,
            Code = account.Code,
            Notes = account.Notes
                .Select(x => x.Text),
            Tags = account.Tags
                .Select(x => x.Name)
                .ToList(),
        };
    }
}
