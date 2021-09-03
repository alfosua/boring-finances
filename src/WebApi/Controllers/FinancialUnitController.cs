using System;
using System.Linq;
using System.Threading.Tasks;
using BoringSoftware.Finances.Core.FinancialUnits;
using BoringSoftware.Finances.Data;
using BoringSoftware.Finances.Dtos.FinancialUnits;
using BoringSoftware.Finances.Entities.FinancialUnits;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BoringSoftware.Finances.WebApi.Controllers
{
    [ApiController]
    [Route("api/v0/financial-units")]
    public class FinancialUnitController : ControllerBase
    {
        #region Dependencies

        private readonly BoringFinancesDbContext _dbContext;
        private readonly IFinancialUnitCruder _financialUnitCruder;
        private readonly IFinancialUnitMapper _financialUnitMapper;
        private readonly ILogger<FinancialUnitController> _logger;

        #endregion Dependencies

        #region Constructor

        public FinancialUnitController(BoringFinancesDbContext dbContext, IFinancialUnitCruder financialUnitCruder, IFinancialUnitMapper financialUnitMapper, ILogger<FinancialUnitController> logger)
        {
            _dbContext = dbContext;
            _financialUnitCruder = financialUnitCruder;
            _financialUnitMapper = financialUnitMapper;
            _logger = logger;
        }

        #endregion Constructor

        #region Actions

        [HttpGet]
        public async Task<IActionResult> GetAllFinancialUnitsAsync()
        {
            var financialUnits = await _dbContext.FinancialUnits.ToListAsync();

            var facades = financialUnits.Select(x => _financialUnitMapper.MapToFinancialUnitCrudFacade(x));

            var readingJsons = facades.Select(x => _financialUnitMapper.MapToFinancialUnitReadingJson(x));

            return Ok(readingJsons);
        }

        [HttpPost]
        public async Task<IActionResult> PostOneFinancialUnitAsync([FromBody] FinancialUnitEditionJson financialUnitEditionJson)
        {
            var facadeToCreate = _financialUnitMapper.MapToFinancialUnitCrudFacade(financialUnitEditionJson);

            var createdFacade = await _financialUnitCruder.CreateOneFromAsync(facadeToCreate);

            var readingJson = _financialUnitMapper.MapToFinancialUnitReadingJson(createdFacade);

            return Created($"/api/v0/financialUnits/{ createdFacade.Id }", readingJson);
        }

        [HttpGet("{financialUnitId}")]
        public async Task<IActionResult> GetOneFinancialUnitAsync([FromRoute] Guid financialUnitId)
        {
            var readFacade = await _financialUnitCruder.ReadOneByIdASync(financialUnitId);

            var readingJson = _financialUnitMapper.MapToFinancialUnitReadingJson(readFacade);
            
            return Ok(readingJson);
        }

        [HttpPut("{financialUnitId}")]
        public async Task<IActionResult> PutOneFinancialUnitAsync([FromRoute] Guid financialUnitId, [FromBody] FinancialUnitEditionJson financialUnitEditionJson)
        {
            var facadeToUpdate = _financialUnitMapper.MapToFinancialUnitCrudFacade(financialUnitEditionJson);

            var updatedFacade = await _financialUnitCruder.UpdateOneByIdFromAsync(financialUnitId, facadeToUpdate);

            var readingJson = _financialUnitMapper.MapToFinancialUnitReadingJson(updatedFacade);

            return Ok(readingJson);
        }

        [HttpDelete("{financialUnitId}")]
        public async Task<IActionResult> DeleteOneFinancialUnitAsync([FromRoute] Guid financialUnitId)
        {
            var updatedFacade = await _financialUnitCruder.DeleteOneByIdASync(financialUnitId);

            var readingJson = _financialUnitMapper.MapToFinancialUnitReadingJson(updatedFacade);

            return Ok(readingJson);
        }

        [HttpGet("types")]
        public async Task<ActionResult> GetAllFinancialUnitTypesAsync()
        {
            var query = await _dbContext.FinancialUnitTypes
                .AsNoTracking()
                .Select(x => new { Id = x.Id })
                .ToListAsync();

            var result = query
                .Select(x => new
                {
                    Id = x.Id,
                    Code = ((FinancialUnitTypeOption)x.Id).ToString(),
                })
                .ToList();

            return Ok(result);
        }

        #endregion Actions
    }
}
