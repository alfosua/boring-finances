using BoringFinances.Operations.WebApi.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace BoringFinances.Operations.WebApi.Operations;

[Route("api/operations")]
[ApiController]
public class OperationController : ControllerBase
{
    private readonly IOperationCruder _operationCruder;
    private readonly IOperationLister _operationLister;

    public OperationController(IOperationCruder operationCruder, IOperationLister operationLister)
    {
        _operationCruder = operationCruder;
        _operationLister = operationLister;
    }

    [HttpPost]
    public async Task<ActionResult<OperationDto>> PostOneOperationAsync([FromBody] OperationDto operationToCreate)
    {
        var operation = await _operationCruder.CreateOneFromAsync(operationToCreate);

        return Ok(operation);
    }

    [HttpGet("{operationId}")]
    public async Task<ActionResult<OperationDto>> GetOneOperationAsync([FromRoute] long operationId)
    {
        var operation = await _operationCruder.ReadOneByIdAsync(operationId);

        return Ok(operation);
    }

    [HttpPut("{operationId}")]
    public async Task<ActionResult<OperationDto>> PutOneOperationAsync([FromRoute] long operationId, [FromBody] OperationDto operationToUpdate)
    {
        var operation = await _operationCruder.UpdateOneByIdFromAsync(operationId, operationToUpdate);

        return Ok(operation);
    }

    [HttpDelete("{operationId}")]
    public async Task<ActionResult<OperationDto>> DeleteOneOperationAsync([FromRoute] long operationId)
    {
        var operation = await _operationCruder.DeleteOneByIdAsync(operationId);

        return Ok(operation);
    }

    [HttpGet]
    public async Task<ActionResult<CountedEnumerable<OperationDto>>> ListManyOperationsAsync([FromQuery] OperationLimitedFilter filter)
    {
        var operations = await _operationLister.ListManyByFilteringAsync(filter);

        return Ok(operations);
    }

    [HttpGet("page")]
    public async Task<ActionResult<PagedEnumerable<OperationDto>>> PageManyOperationsAsync([FromQuery] OperationPagedFilter filter)
    {
        var operations = await _operationLister.PageManyByFilteringAsync(filter);

        return Ok(operations);
    }
}
