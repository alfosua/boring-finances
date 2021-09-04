using System;
using System.Threading.Tasks;

namespace BoringSoftware.Finances.Core.FinancialUnits
{
    public interface IFinancialUnitCruder
    {
        Task<FinancialUnitCrudFacade> CreateOneFromAsync(FinancialUnitCrudFacade facade);

        Task<FinancialUnitCrudFacade> ReadOneByIdASync(Guid financialUnitId);

        Task<FinancialUnitCrudFacade> UpdateOneByIdFromAsync(Guid financialUnitId, FinancialUnitCrudFacade facade);

        Task<FinancialUnitCrudFacade> DeleteOneByIdASync(Guid financialUnitId);
    }
}
