using BoringFinances.Operations.Data.Operations;
using System.Linq.Expressions;

namespace BoringFinances.Operations.WebApi.Operations;

public interface IOperationDtoMapper
{
    Expression<Func<Operation, OperationDto>> MostRecentVersionMappingExpression { get; }

    OperationDto CreateOperationDtoFrom(Operation operation);
}
