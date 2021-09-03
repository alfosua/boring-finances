using System;
using System.Threading.Tasks;

namespace BoringSoftware.Finances.Core.Operations
{
    public interface IOperationCruder
    {
        Task<OperationCrudFacade> CreateOneFromAsync(OperationCrudFacade payload);

        Task<OperationCrudFacade> ReadOneByIdAsync(Guid operationId);

        Task<OperationCrudFacade> UpdateOneByIdFromAsync(Guid operationId, OperationCrudFacade payload);

        Task<OperationCrudFacade> DeleteOneByIdAsync(Guid operationId);
    }
}
