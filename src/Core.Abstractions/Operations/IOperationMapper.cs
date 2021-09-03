using BoringSoftware.Finances.Dtos.Operations;
using BoringSoftware.Finances.Entities.Operations;
using System.Threading.Tasks;

namespace BoringSoftware.Finances.Core.Operations
{
    public interface IOperationMapper
    {
        OperationCrudFacade MapToOperationCrudFacade(Operation operation, OperationCrudFacade operationCrudFacade = null);
        
        Task<OperationCrudFacade> MapToOperationCrudFacadeAsync(OperationEditionJson operationEditionJson, OperationCrudFacade operationCrudFacade = null);

        OperationReadingJson MapToOperationReadingJson(OperationCrudFacade operationCrudFacade, OperationReadingJson operationReadingJson = null);
    }
}
