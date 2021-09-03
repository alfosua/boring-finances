using BoringSoftware.Finances.Entities.Operations;
using System;
using System.Collections.Generic;

namespace BoringSoftware.Finances.Core.Operations
{
    public class OperationEntryTypeResolver : IOperationEntryTypeResolver
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
            if (byte.TryParse(stringValue, out var operationEntryTypeId))
            {
                return operationEntryTypeId;
            }
            else if (Enum.TryParse<OperationEntryTypeOption>(stringValue, out var operationEntryTypeOption))
            {
                return (byte)operationEntryTypeOption;
            }
            else
            {
                throw new InvalidOperationException($"Unable to parse '{ stringValue}' to a valid account type ID.");
            }
        }

        #endregion Services
    }
}
