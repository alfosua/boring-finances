using BoringSoftware.Finances.Entities.Accounts;
using System;
using System.Collections.Generic;

namespace BoringSoftware.Finances.Core.Accounts
{
    public class AccountTypeResolver : IAccountTypeResolver
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
            if (byte.TryParse(stringValue, out var accountTypeId))
            {
                return accountTypeId;
            }
            else if (Enum.TryParse<AccountTypeOption>(stringValue, out var accountTypeOption))
            {
                return (byte)accountTypeOption;
            }
            else
            {
                throw new InvalidOperationException($"Unable to parse '{ stringValue}' to a valid account type ID.");
            }
        }

        #endregion Services
    }
}
