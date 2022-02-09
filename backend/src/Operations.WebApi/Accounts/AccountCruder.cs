using BoringFinances.Operations.Data;
using BoringFinances.Operations.Data.Accounts;
using BoringFinances.Operations.WebApi.Utilities;
using Microsoft.EntityFrameworkCore;

namespace BoringFinances.Operations.WebApi.Accounts;

public class AccountCruder : IAccountCruder
{
    private readonly OperationDbContext _dbContext;
    private readonly ITemporalEntityDomainer _temporalEntityDomain;
    private readonly INoteDomainer _noteDomain;
    private readonly ITagDomainer _tagDomain;
    private readonly IAccountDtoMapper _accountDtoMapper;

    public AccountCruder(
        OperationDbContext dbContext,
        ITemporalEntityDomainer temporalEntityDomain,
        INoteDomainer noteDomain,
        ITagDomainer tagDomain,
        IAccountDtoMapper accountDtoMapper)
    {
        _dbContext = dbContext;
        _temporalEntityDomain = temporalEntityDomain;
        _noteDomain = noteDomain;
        _tagDomain = tagDomain;
        _accountDtoMapper = accountDtoMapper;
    }

    public async Task<AccountDto> CreateOneFromAsync(AccountDto accountDto)
    {
        var validEffectTimestamp = DateTime.UtcNow;

        var accountToCreate = new Account();
        var accountTimestamped = _temporalEntityDomain.TimestampCreationTo(accountToCreate, validEffectTimestamp);
        var accountWithMapping = MapToAccount(accountDto, accountTimestamped);
        var accountWithNotes = _noteDomain.DecorateWithNotesTo(accountWithMapping, accountDto.Notes);
        var accountWithTags = await _tagDomain.DecorateWithTagsToAsync(accountWithNotes, accountDto.Tags);
        
        var accountCreated = await _dbContext.Piping(accountWithTags).AddAndSaveChangesAsync();
        var result = _accountDtoMapper.CreateAccountDtoFrom(accountCreated);

        return result;
    }

    public async Task<AccountDto> ReadOneByIdAsync(long accountId)
    {
        var result = await _dbContext.Accounts
            .Where(x => x.AccountId == accountId && !x.Deleted.HasValue)
            .Select(_accountDtoMapper.SelectExpression)
            .SingleOrDefaultAsync();

        if (result is null)
        {
            throw new KeyNotFoundException(nameof(accountId));
        }

        return result;
    }

    public async Task<AccountDto> UpdateOneByIdFromAsync(long accountId, AccountDto accountDto)
    {
        var accountQueried = await FindAccountAsync(accountId);
        var accountWithMapping = MapToAccount(accountDto, accountQueried);
        var accountWithNotes = _noteDomain.DecorateWithNotesTo(accountWithMapping, accountDto.Notes);
        var accountWithTags = await _tagDomain.DecorateWithTagsToAsync(accountWithNotes, accountDto.Tags);

        var accountUpdated = await _dbContext.Piping(accountWithTags).UpdateAndSaveChangesAsync();
        var result = _accountDtoMapper.CreateAccountDtoFrom(accountUpdated);

        return result;
    }

    public async Task<AccountDto> DeleteOneByIdAsync(long accountId)
    {
        var validEffectTimestamp = DateTime.UtcNow;

        var accountQueried = await FindAccountAsync(accountId);
        var accountTimestamped = _temporalEntityDomain.TimestampDeletionTo(accountQueried, validEffectTimestamp);

        var accountDeleted = await _dbContext.Piping(accountTimestamped).UpdateAndSaveChangesAsync();
        var result = _accountDtoMapper.CreateAccountDtoFrom(accountDeleted);

        return result;
    }

    private async Task<Account> FindAccountAsync(long accountId)
    {
        var accountToUpdate = await _dbContext.Accounts
            .Include(x => x.Notes)
            .Include(x => x.Tags)
            .FirstOrDefaultAsync(x => x.AccountId == accountId && !x.Deleted.HasValue);

        if (accountToUpdate is null)
        {
            throw new KeyNotFoundException(nameof(accountId));
        }

        return accountToUpdate;
    }

    public Account MapToAccount(AccountDto accountDto, Account account)
    {
        account ??= new();

        account.Title = accountDto.Title;
        account.AccountTypeId = accountDto.AccountTypeId;
        account.Code = accountDto.Code;

        return account;
    }
}
