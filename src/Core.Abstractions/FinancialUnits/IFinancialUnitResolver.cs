using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BoringSoftware.Finances.Core.FinancialUnits
{
    public interface IFinancialUnitResolver
    {
        Task<IReadOnlyDictionary<string, Guid>> ResolveManyFromAsync(IEnumerable<string> stringValues);

        Task<Guid> ResolveOneFromAsync(string stringValue);
    }
}
