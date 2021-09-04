using System;
using System.Collections.Generic;

namespace BoringSoftware.Finances.Core.FinancialUnits
{
    public interface IFinancialUnitTypeResolver
    {
        IReadOnlyDictionary<string, byte> ResolveManyFrom(IEnumerable<string> stringValues);

        byte ResolveOneFrom(string stringValue);
    }
}
