using BoringSoftware.Finances.Dtos.FinancialUnits;
using BoringSoftware.Finances.Entities.FinancialUnits;

namespace BoringSoftware.Finances.Core.FinancialUnits
{
    public interface IFinancialUnitMapper
    {
        public FinancialUnitCrudFacade MapToFinancialUnitCrudFacade(FinancialUnitEditionJson financialUnitEditionJson, FinancialUnitCrudFacade financialUnitCrudFacade = null);

        FinancialUnitCrudFacade MapToFinancialUnitCrudFacade(FinancialUnit financialUnit, FinancialUnitCrudFacade financialUnitCrudFacade = null);

        public FinancialUnitReadingJson MapToFinancialUnitReadingJson(FinancialUnitCrudFacade financialUnitCrudFacade, FinancialUnitReadingJson financialUnitReadingJson = null);
    }
}
