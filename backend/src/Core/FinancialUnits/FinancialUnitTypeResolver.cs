using BoringSoftware.Finances.Entities.FinancialUnits;
using System;
using System.Collections.Generic;

namespace BoringSoftware.Finances.Core.FinancialUnits
{
    public class FinancialUnitTypeResolver : IFinancialUnitTypeResolver
    {
        #region Services

        public IReadOnlyDictionary<string, byte> ResolveManyFrom(IEnumerable<string> stringValues)
        {
            var result = new Dictionary<string, byte>();

            foreach (var value in stringValues)
            {
                result[value] = ResolveOneFrom(value);
            }

            return result;
        }

        public byte ResolveOneFrom(string stringValue)
        {
            if (byte.TryParse(stringValue, out var financialUnitTypeId))
            {
                return financialUnitTypeId;
            }
            else if (Enum.TryParse<FinancialUnitTypeOption>(stringValue, out var financialUnitTypeOption))
            {
                return (byte)financialUnitTypeOption;
            }
            else
            {
                throw new InvalidOperationException($"Unable to parse '{ stringValue}' to a valid financialUnit type ID.");
            }
        }

        #endregion Services
    }
}
