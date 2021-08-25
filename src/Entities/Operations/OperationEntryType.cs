using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoringSoftware.Finances.Entities.Operations
{
    public class OperationEntryType
    {
        #region Mapped Properties
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }

        [Required, StringLength(6)]
        public string Code { get; set; }

        #endregion Mapped Properties

        #region Navigation Properties

        public ICollection<OperationEntry> OperationEntries { get; set; }

        #endregion Navigation Properties

        #region Helper Properties

        public OperationEntryTypeOption Option => (OperationEntryTypeOption)Id;
     
        #endregion Helper Properties
    }
}
