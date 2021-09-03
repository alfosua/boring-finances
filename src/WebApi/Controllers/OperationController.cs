using System;
using System.Linq;
using System.Threading.Tasks;
using BoringSoftware.Finances.Core.Operations;
using BoringSoftware.Finances.Data;
using BoringSoftware.Finances.Dtos.Operations;
using BoringSoftware.Finances.Entities.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BoringSoftware.Finances.WebApi.Controllers
{
    [ApiController]
    [Route("api/v0/operations")]
    public class OperationController : ControllerBase
    {
        #region Dependencies

        private readonly IOperationCruder _operationCruder;
        private readonly IOperationMapper _operationMapper;
        private readonly BoringFinancesDbContext _dbContext;
        private readonly ILogger<OperationController> _logger;

        #endregion Dependencies

        #region Constructor

        public OperationController(IOperationCruder operationCruder, IOperationMapper operationMapper, BoringFinancesDbContext dbContext, ILogger<OperationController> logger)
        {
            _operationCruder = operationCruder;
            _operationMapper = operationMapper;
            _dbContext = dbContext;
            _logger = logger;
        }

        #endregion Constructor

        #region Actions

        [HttpGet]
        public async Task<IActionResult> GetAllOperationsAsync()
        {
            var operations = await _dbContext.Operations
                .AsNoTracking()
                .Include(x => x.Entries).ThenInclude(x => x.Notes)
                .Include(x => x.Notes)
                .ToListAsync();

            var result = operations
                .Select(x => _operationMapper.MapToOperationCrudFacade(x))
                .Select(x => _operationMapper.MapToOperationReadingJson(x));

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostOneOperationAsync([FromBody] OperationEditionJson operationEditionJson)
        {
            var facadeToCreate = await _operationMapper.MapToOperationCrudFacadeAsync(operationEditionJson);

            var createdFacade = await _operationCruder.CreateOneFromAsync(facadeToCreate);

            var result = _operationMapper.MapToOperationReadingJson(createdFacade);

            return Created($"/api/v0/operations/{ result.Id }", result);
        }

        [HttpGet("{operationId}")]
        public async Task<IActionResult> GetOneOperationAsync([FromRoute] Guid operationId)
        {
            var readFacade = await _operationCruder.ReadOneByIdAsync(operationId);

            var result = _operationMapper.MapToOperationReadingJson(readFacade);

            return Ok(result);
        }

        [HttpPut("{operationId}")]
        public async Task<IActionResult> PutOneOperationAsync([FromRoute] Guid operationId, [FromBody] OperationEditionJson operationEditionJson)
        {
            var facadeToUpdate = await _operationMapper.MapToOperationCrudFacadeAsync(operationEditionJson);

            var updatedFacade = await _operationCruder.UpdateOneByIdFromAsync(operationId, facadeToUpdate);

            var result = _operationMapper.MapToOperationReadingJson(updatedFacade);

            return Ok(result);
        }

        [HttpDelete("{operationId}")]
        public async Task<IActionResult> DeleteOneOperationAsync([FromRoute] Guid operationId)
        {
            var deletedFacade = await _operationCruder.DeleteOneByIdAsync(operationId);

            var result = _operationMapper.MapToOperationReadingJson(deletedFacade);

            return Ok(result);
        }

        [HttpGet("entries/types")]
        public async Task<ActionResult> GetAllOperationEntryTypesAsync()
        {
            var query = await _dbContext.OperationEntryTypes
                .AsNoTracking()
                .Select(x => new { Id = x.Id })
                .ToListAsync();

            var result = query
                .Select(x => new
                {
                    Id = x.Id,
                    Code = ((OperationEntryTypeOption)x.Id).ToString(),
                })
                .ToList();

            return Ok(result);
        }

        #endregion Actions
    }
}
