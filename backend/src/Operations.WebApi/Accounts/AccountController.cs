using BoringFinances.Operations.WebApi.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace BoringFinances.Operations.WebApi.Accounts;

[Route("api/accounts")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountCruder _accountCruder;
    private readonly IAccountLister _accountLister;

    public AccountController(IAccountCruder accountCruder, IAccountLister accountLister)
    {
        _accountCruder = accountCruder;
        _accountLister = accountLister;
    }

    [HttpPost]
    public async Task<ActionResult<AccountDto>> PostOneAccountAsync([FromBody] AccountDto accountToCreate)
    {
        var account = await _accountCruder.CreateOneFromAsync(accountToCreate);

        return Ok(account);
    }

    [HttpGet("{accountId}")]
    public async Task<ActionResult<AccountDto>> GetOneAccountAsync([FromRoute] long accountId)
    {
        var account = await _accountCruder.ReadOneByIdAsync(accountId);

        return Ok(account);
    }

    [HttpPut("{accountId}")]
    public async Task<ActionResult<AccountDto>> PutOneAccountAsync([FromRoute] long accountId, [FromBody] AccountDto accountToUpdate)
    {
        var account = await _accountCruder.UpdateOneByIdFromAsync(accountId, accountToUpdate);

        return Ok(account);
    }

    [HttpDelete("{accountId}")]
    public async Task<ActionResult<AccountDto>> DeleteOneAccountAsync([FromRoute] long accountId)
    {
        var account = await _accountCruder.DeleteOneByIdAsync(accountId);

        return Ok(account);
    }

    [HttpGet]
    public async Task<ActionResult<CountedEnumerable<AccountDto>>> ListManyAccountsAsync([FromQuery] AccountLimitedFilter filter)
    {
        var accounts = await _accountLister.ListManyByFilteringAsync(filter);

        return Ok(accounts);
    }

    [HttpGet("page")]
    public async Task<ActionResult<PagedEnumerable<AccountDto>>> PageManyAccountsAsync([FromQuery] AccountPagedFilter filter)
    {
        var accounts = await _accountLister.PageManyByFilteringAsync(filter);

        return Ok(accounts);
    }
}
