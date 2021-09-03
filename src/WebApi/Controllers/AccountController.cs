using System;
using System.Linq;
using System.Threading.Tasks;
using BoringSoftware.Finances.Core.Accounts;
using BoringSoftware.Finances.Data;
using BoringSoftware.Finances.Dtos.Accounts;
using BoringSoftware.Finances.Entities.Accounts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BoringSoftware.Finances.WebApi.Controllers
{
    [ApiController]
    [Route("api/v0/accounts")]
    public class AccountController : ControllerBase
    {
        #region Dependencies

        private readonly BoringFinancesDbContext _dbContext;
        private readonly IAccountCruder _accountCruder;
        private readonly IAccountMapper _accountMapper;
        private readonly ILogger<AccountController> _logger;

        #endregion Dependencies

        #region Constructor

        public AccountController(BoringFinancesDbContext dbContext, IAccountCruder accountCruder, IAccountMapper accountMapper, ILogger<AccountController> logger)
        {
            _dbContext = dbContext;
            _accountCruder = accountCruder;
            _accountMapper = accountMapper;
            _logger = logger;
        }

        #endregion Constructor

        #region Actions

        [HttpGet]
        public async Task<IActionResult> GetAllAccountsAsync()
        {
            var accounts = await _dbContext.Accounts.ToListAsync();

            var facades = accounts.Select(x => _accountMapper.MapToAccountCrudFacade(x));

            var readingJsons = facades.Select(x => _accountMapper.MapToAccountReadingJson(x));

            return Ok(readingJsons);
        }

        [HttpDelete("{accountId}")]
        public async Task<IActionResult> DeleteOneAccountAsync([FromRoute] Guid accountId)
        {
            var updatedFacade = await _accountCruder.DeleteOneByIdASync(accountId);

            var readingJson = _accountMapper.MapToAccountReadingJson(updatedFacade);

            return Ok(readingJson);
        }

        [HttpGet("{accountId}")]
        public async Task<IActionResult> GetOneAccountAsync([FromRoute] Guid accountId)
        {
            var readFacade = await _accountCruder.ReadOneByIdASync(accountId);

            var readingJson = _accountMapper.MapToAccountReadingJson(readFacade);

            return Ok(readingJson);
        }

        [HttpPost]
        public async Task<IActionResult> PostOneAccountAsync([FromBody] AccountEditionJson accountEditionJson)
        {
            var facadeToCreate = _accountMapper.MapToAccountCrudFacade(accountEditionJson);

            var createdFacade = await _accountCruder.CreateOneFromAsync(facadeToCreate);

            var readingJson = _accountMapper.MapToAccountReadingJson(createdFacade);

            return Created($"/api/v0/accounts/{ createdFacade.Id }", readingJson);
        }

        [HttpPut("{accountId}")]
        public async Task<IActionResult> PutOneAccountAsync([FromRoute] Guid accountId, [FromBody] AccountEditionJson accountEditionJson)
        {
            var facadeToUpdate = _accountMapper.MapToAccountCrudFacade(accountEditionJson);

            var updatedFacade = await _accountCruder.UpdateOneByIdFromAsync(accountId, facadeToUpdate);

            var readingJson = _accountMapper.MapToAccountReadingJson(updatedFacade);

            return Ok(readingJson);
        }

        [HttpGet("types")]
        public async Task<ActionResult> GetAllAccountTypesAsync()
        {
            var query = await _dbContext.AccountTypes
                .AsNoTracking()
                .Select(x => new { Id = x.Id })
                .ToListAsync();

            var result = query
                .Select(x => new
                {
                    Id = x.Id,
                    Code = ((AccountTypeOption)x.Id).ToString(),
                })
                .ToList();

            return Ok(result);
        }

        #endregion Actions
    }
}
