namespace BoringFinances.Operations.WebApi.Operations;

public interface IOperationCruder
{
    Task<OperationDto> ReadOneByIdAsync(long operationId);
    
    Task<OperationDto> CreateOneFromAsync(OperationDto operation);
    
    Task<OperationDto> UpdateOneByIdFromAsync(long operationId, OperationDto operationToUpdate);

    Task<OperationDto> DeleteOneByIdAsync(long operationId);
}
