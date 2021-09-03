using System;
using System.Collections.Generic;

namespace BoringSoftware.Finances.Core.Accounts
{
    public interface IAccountTypeResolver
    {
        IReadOnlyDictionary<string, byte> ResolveManyFrom(IEnumerable<string> stringValues);

        byte ResolveOneFrom(string stringValue);
    }
}
