using System.Collections.Generic;

namespace BoringSoftware.Finances.Core.Operations
{
    public interface IOperationEntryTypeResolver
    {
        IReadOnlyDictionary<string, byte> ResolveManyFrom(IEnumerable<string> stringValues);

        byte ResolveOneFrom(string stringValue);
    }
}
