using BoringSoftware.Finances.Dtos.Utils;
using System;

namespace BoringSoftware.Finances.Dtos.Accounts
{
    public class AccountReadingJson
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Kebab { get; set; }

        public IdCodeHrefGroup<byte> AccountType { get; set; }
    }
}
