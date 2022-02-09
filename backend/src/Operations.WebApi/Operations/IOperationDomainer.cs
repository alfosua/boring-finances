using BoringFinances.Operations.Data.Operations;

namespace BoringFinances.Operations.WebApi.Operations;

public interface IOperationDomainer
{
    Operation DecorateWithEntriesTo(Operation operation, IEnumerable<OperationEntryDto> operationEntryDtos, DateTime effectTimestamp);
}
