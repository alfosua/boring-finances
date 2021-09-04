using BoringSoftware.Finances.Dtos.Utils;
using System;

namespace BoringSoftware.Finances.Dtos.FinancialUnits
{
    public class FinancialUnitReadingJson
    {
        public Guid Id { get; set; }

        public string Kebab { get; set; }

        public IdCodeHrefGroup<byte> FinancialUnitType { get; set; }
    }
}
