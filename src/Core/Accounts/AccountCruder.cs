using BoringSoftware.Finances.Data;
using BoringSoftware.Finances.Entities.Accounts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BoringSoftware.Finances.Core.Accounts
{
    public class AccountCruder : IAccountCruder
    {
        #region Dependencies

        private readonly BoringFinancesDbContext _dbContext;

        #endregion Dependencies

        #region Constructor

        public AccountCruder(BoringFinancesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion Constructor

        #region Services

        public async Task<AccountCrudFacade> CreateOneFromAsync(AccountCrudFacade facade)
        {
            var accountToCreate = new Account { Id = Guid.NewGuid() };

            var accountWithMapping = MapToAccount(facade, accountToCreate);

            var accountAddedInDb = await AddAccountInDbAsync(accountWithMapping);

            var result = MapToAccountCrudFacade(accountAddedInDb);

            return result;
        }

        public async Task<AccountCrudFacade> ReadOneByIdASync(Guid accountId)
        {
            var accountToRead = await _dbContext.Accounts.FirstOrDefaultAsync(x => x.Id == accountId);

            if (accountToRead is null)
            {
                throw new KeyNotFoundException("Account does not exist");
            }

            var result = MapToAccountCrudFacade(accountToRead);

            return result;
        }

        public async Task<AccountCrudFacade> UpdateOneByIdFromAsync(Guid accountId, AccountCrudFacade facade)
        {
            var accountToUpdate = new Account { Id = accountId };

            var accountWithModifications = await _dbContext.ModifyAndSaveChangesAsync(accountToUpdate, x => MapToAccount(facade, x));

            var result = MapToAccountCrudFacade(accountWithModifications);

            return result;
        }

        public async Task<AccountCrudFacade> DeleteOneByIdASync(Guid accountId)
        {
            var accountToDelete = new Account { Id = accountId };

            var accountAlreadyDeleted = await _dbContext.DeleteAndSaveChangesAsync(accountToDelete);

            var result = MapToAccountCrudFacade(accountAlreadyDeleted);

            return result;
        }

        #endregion Services

        #region Processing

        private async Task<Account> AddAccountInDbAsync(Account account)
        {
            _dbContext.Accounts.Add(account);

            await _dbContext.SaveChangesAsync();

            return account;
        }

        private Account MapToAccount(AccountCrudFacade accountCrudFacade, Account account = null)
        {
            account ??= new();

            account.Title = accountCrudFacade.Title;
            account.Kebab = accountCrudFacade.Kebab;
            account.AccountTypeId = accountCrudFacade.AccountTypeId;

            return account;
        }

        private AccountCrudFacade MapToAccountCrudFacade(Account account, AccountCrudFacade accountCrudFacade = null)
        {
            accountCrudFacade ??= new();

            accountCrudFacade.Id = account.Id;
            accountCrudFacade.Title = account.Title;
            accountCrudFacade.Kebab = account.Kebab;
            accountCrudFacade.AccountTypeId = account.AccountTypeId;

            return accountCrudFacade;
        }

        #endregion Processing
    }
}
