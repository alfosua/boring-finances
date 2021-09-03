using BoringSoftware.Finances.Dtos.Accounts;
using BoringSoftware.Finances.Dtos.Utils;
using BoringSoftware.Finances.Entities.Accounts;

namespace BoringSoftware.Finances.Core.Accounts
{
    public class AccountMapper : IAccountMapper
    {
        private readonly IAccountTypeResolver _accountTypeResolver;

        public AccountMapper(IAccountTypeResolver accountTypeResolver)
        {
            _accountTypeResolver = accountTypeResolver;
        }

        public AccountCrudFacade MapToAccountCrudFacade(AccountEditionJson accountCreationJson, AccountCrudFacade accountCrudFacade = null)
        {
            accountCrudFacade ??= new AccountCrudFacade();

            accountCrudFacade.Title = accountCreationJson.Title;
            accountCrudFacade.Kebab = accountCreationJson.Kebab;
            accountCrudFacade.AccountTypeId = _accountTypeResolver.ResolveOneFrom(accountCreationJson.AccountType);

            return accountCrudFacade;
        }

        public AccountReadingJson MapToAccountReadingJson(AccountCrudFacade accountCrudFacade, AccountReadingJson accountReadingJson = null)
        {
            accountReadingJson ??= new AccountReadingJson();

            accountReadingJson.Id = accountCrudFacade.Id.GetValueOrDefault();
            accountReadingJson.Title = accountCrudFacade.Title;
            accountReadingJson.Kebab = accountCrudFacade.Kebab;
            accountReadingJson.AccountType = new IdCodeHrefGroup<byte>
            {
                Id = accountCrudFacade.AccountTypeId,
                Code = ((AccountTypeOption)accountCrudFacade.AccountTypeId).ToString(),
                Href = $"/api/v0/accounts/types/{ accountCrudFacade.AccountTypeId }",
            };

            return accountReadingJson;
        }

        public AccountCrudFacade MapToAccountCrudFacade(Account account, AccountCrudFacade accountCrudFacade = null)
        {
            accountCrudFacade ??= new();

            accountCrudFacade.Id = account.Id;
            accountCrudFacade.Title = account.Title;
            accountCrudFacade.Kebab = account.Kebab;
            accountCrudFacade.AccountTypeId = account.AccountTypeId;

            return accountCrudFacade;
        }
    }
}
