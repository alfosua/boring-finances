using BoringSoftware.Finances.Dtos.FinancialUnits;
using BoringSoftware.Finances.Dtos.Utils;
using BoringSoftware.Finances.Entities.FinancialUnits;

namespace BoringSoftware.Finances.Core.FinancialUnits
{
    public class FinancialUnitMapper : IFinancialUnitMapper
    {
        private readonly IFinancialUnitTypeResolver _financialUnitTypeResolver;

        public FinancialUnitMapper(IFinancialUnitTypeResolver financialUnitTypeResolver)
        {
            _financialUnitTypeResolver = financialUnitTypeResolver;
        }

        public FinancialUnitCrudFacade MapToFinancialUnitCrudFacade(FinancialUnitEditionJson financialUnitCreationJson, FinancialUnitCrudFacade financialUnitCrudFacade = null)
        {
            financialUnitCrudFacade ??= new FinancialUnitCrudFacade();

            financialUnitCrudFacade.Kebab = financialUnitCreationJson.Kebab;
            financialUnitCrudFacade.FinancialUnitTypeId = _financialUnitTypeResolver.ResolveOneFrom(financialUnitCreationJson.FinancialUnitType);

            return financialUnitCrudFacade;
        }

        public FinancialUnitReadingJson MapToFinancialUnitReadingJson(FinancialUnitCrudFacade financialUnitCrudFacade, FinancialUnitReadingJson financialUnitReadingJson = null)
        {
            financialUnitReadingJson ??= new FinancialUnitReadingJson();

            financialUnitReadingJson.Id = financialUnitCrudFacade.Id.GetValueOrDefault();
            financialUnitReadingJson.Kebab = financialUnitCrudFacade.Kebab;
            financialUnitReadingJson.FinancialUnitType = new IdCodeHrefGroup<byte>
            {
                Id = financialUnitCrudFacade.FinancialUnitTypeId,
                Code = ((FinancialUnitTypeOption)financialUnitCrudFacade.FinancialUnitTypeId).ToString(),
                Href = $"/api/v0/financial-units/types/{ financialUnitCrudFacade.FinancialUnitTypeId }",
            };

            return financialUnitReadingJson;
        }

        public FinancialUnitCrudFacade MapToFinancialUnitCrudFacade(FinancialUnit financialUnit, FinancialUnitCrudFacade financialUnitCrudFacade = null)
        {
            financialUnitCrudFacade ??= new();

            financialUnitCrudFacade.Id = financialUnit.Id;
            financialUnitCrudFacade.Kebab = financialUnit.Kebab;
            financialUnitCrudFacade.FinancialUnitTypeId = financialUnit.FinancialUnitTypeId;

            return financialUnitCrudFacade;
        }
    }
}
