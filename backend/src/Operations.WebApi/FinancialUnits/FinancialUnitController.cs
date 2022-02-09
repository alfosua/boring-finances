using BoringFinances.Operations.WebApi.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace BoringFinances.Operations.WebApi.FinancialUnits;

[Route("api/financial-units")]
[ApiController]
public class FinancialUnitController : ControllerBase
{
    private readonly IFinancialUnitCruder _financialUnitCruder;
    private readonly IFinancialUnitLister _financialUnitLister;

    public FinancialUnitController(IFinancialUnitCruder financialUnitCruder, IFinancialUnitLister financialUnitLister)
    {
        _financialUnitCruder = financialUnitCruder;
        _financialUnitLister = financialUnitLister;
    }

    [HttpPost]
    public async Task<ActionResult<FinancialUnitDto>> PostOneFinancialUnitAsync([FromBody] FinancialUnitDto financialUnitToCreate)
    {
        var financialUnit = await _financialUnitCruder.CreateOneFromAsync(financialUnitToCreate);

        return Ok(financialUnit);
    }

    [HttpGet("{financialUnitId}")]
    public async Task<ActionResult<FinancialUnitDto>> GetOneFinancialUnitAsync([FromRoute] long financialUnitId)
    {
        var financialUnit = await _financialUnitCruder.ReadOneByIdAsync(financialUnitId);

        return Ok(financialUnit);
    }

    [HttpPut("{financialUnitId}")]
    public async Task<ActionResult<FinancialUnitDto>> PutOneFinancialUnitAsync([FromRoute] long financialUnitId, [FromBody] FinancialUnitDto financialUnitToUpdate)
    {
        var financialUnit = await _financialUnitCruder.UpdateOneByIdFromAsync(financialUnitId, financialUnitToUpdate);

        return Ok(financialUnit);
    }

    [HttpDelete("{financialUnitId}")]
    public async Task<ActionResult<FinancialUnitDto>> DeleteOneFinancialUnitAsync([FromRoute] long financialUnitId)
    {
        var financialUnit = await _financialUnitCruder.DeleteOneByIdAsync(financialUnitId);

        return Ok(financialUnit);
    }

    [HttpGet]
    public async Task<ActionResult<CountedEnumerable<FinancialUnitDto>>> ListManyFinancialUnitsAsync([FromQuery] FinancialUnitLimitedFilter filter)
    {
        var financialUnits = await _financialUnitLister.ListManyByFilteringAsync(filter);

        return Ok(financialUnits);
    }

    [HttpGet("page")]
    public async Task<ActionResult<PagedEnumerable<FinancialUnitDto>>> PageManyFinancialUnitsAsync([FromQuery] FinancialUnitPagedFilter filter)
    {
        var financialUnits = await _financialUnitLister.PageManyByFilteringAsync(filter);

        return Ok(financialUnits);
    }
}
