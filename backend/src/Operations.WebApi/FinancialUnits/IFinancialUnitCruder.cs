namespace BoringFinances.Operations.WebApi.FinancialUnits;

public interface IFinancialUnitCruder
{
    Task<FinancialUnitDto> ReadOneByIdAsync(long financialUnitId);
    
    Task<FinancialUnitDto> CreateOneFromAsync(FinancialUnitDto financialUnit);
    
    Task<FinancialUnitDto> UpdateOneByIdFromAsync(long financialUnitId, FinancialUnitDto financialUnitToUpdate);

    Task<FinancialUnitDto> DeleteOneByIdAsync(long financialUnitId);
}
