using BoringSoftware.Finances.Entities.Operations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BoringSoftware.Finances.Entities.Accounts
{
    public class Account
    {
        #region Mapped Properties

        [Key]
        public Guid Id { get; set; }

        public string Kebab { get; set; }

        [Required]
        public string Title { get; set; }

        public byte AccountTypeId { get; set; }

        #endregion Mapped Properties
        
        #region Navigation Properties

        public AccountType Type { get; set; }

        public ICollection<OperationEntry> OperationEntries { get; set; }
     
        #endregion Navigation Properties
    }
}
