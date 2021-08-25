using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoringSoftware.Finances.Entities.Accounts
{
    public class AccountType
    {
        #region Mapped Properties

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }

        [Required, StringLength(9)]
        public string Code { get; set; }

        #endregion Mapped Properties

        #region Navigation Properties

        public ICollection<Account> Accounts { get; set; }

        #endregion Navigation Properties
        
        #region Helper Properties

        public AccountTypeOption Option => (AccountTypeOption)Id;

        #endregion Helper Properties
    }
}
