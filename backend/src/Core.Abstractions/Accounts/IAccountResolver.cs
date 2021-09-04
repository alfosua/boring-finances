using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BoringSoftware.Finances.Core.Accounts
{
    public interface IAccountResolver
    {
        Task<IReadOnlyDictionary<string, Guid>> ResolveManyFromAsync(IEnumerable<string> stringValues);

        Task<Guid> ResolveOneFromAsync(string stringValue);
    }
}
