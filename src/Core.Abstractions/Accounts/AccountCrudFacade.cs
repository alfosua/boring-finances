using System;

namespace BoringSoftware.Finances.Core.Accounts
{
    public class AccountCrudFacade
    {
        public Guid? Id { get; set; }

        public string Title { get; set; }

        public string Kebab { get; set; }

        public byte AccountTypeId { get; set; }
    }
}
