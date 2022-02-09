namespace BoringFinances.Operations.WebApi.Accounts;

public interface IAccountCruder
{
    Task<AccountDto> ReadOneByIdAsync(long accountId);
    
    Task<AccountDto> CreateOneFromAsync(AccountDto account);
    
    Task<AccountDto> UpdateOneByIdFromAsync(long accountId, AccountDto accountToUpdate);

    Task<AccountDto> DeleteOneByIdAsync(long accountId);
}
