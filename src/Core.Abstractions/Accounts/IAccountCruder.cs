using System;
using System.Threading.Tasks;

namespace BoringSoftware.Finances.Core.Accounts
{
    public interface IAccountCruder
    {
        Task<AccountCrudFacade> CreateOneFromAsync(AccountCrudFacade facade);

        Task<AccountCrudFacade> ReadOneByIdASync(Guid accountId);

        Task<AccountCrudFacade> UpdateOneByIdFromAsync(Guid accountId, AccountCrudFacade facade);

        Task<AccountCrudFacade> DeleteOneByIdASync(Guid accountId);
    }
}
