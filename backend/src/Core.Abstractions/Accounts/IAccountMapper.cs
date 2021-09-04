using BoringSoftware.Finances.Dtos.Accounts;
using BoringSoftware.Finances.Entities.Accounts;

namespace BoringSoftware.Finances.Core.Accounts
{
    public interface IAccountMapper
    {
        public AccountCrudFacade MapToAccountCrudFacade(AccountEditionJson accountEditionJson, AccountCrudFacade accountCrudFacade = null);

        AccountCrudFacade MapToAccountCrudFacade(Account account, AccountCrudFacade accountCrudFacade = null);

        public AccountReadingJson MapToAccountReadingJson(AccountCrudFacade accountCrudFacade, AccountReadingJson accountReadingJson = null);
    }
}
