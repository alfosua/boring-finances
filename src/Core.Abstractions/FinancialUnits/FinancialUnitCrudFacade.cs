using System;

namespace BoringSoftware.Finances.Core.FinancialUnits
{
    public class FinancialUnitCrudFacade
    {
        public Guid? Id { get; set; }

        public string Kebab { get; set; }

        public byte FinancialUnitTypeId { get; set; }
    }
}
