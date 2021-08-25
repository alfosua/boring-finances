using System;
using System.ComponentModel.DataAnnotations;

namespace BoringSoftware.Finances.Entities.Operations
{
    public class OperationEntryNote
    {
        #region Mapped Properties

        [Key]
        public Guid Id { get; set; }

        public Guid OperationEntryId { get; set; }

        [Required]
        public string Content { get; set; }

        #endregion Mapped Properties

        #region Navigation Properties

        public OperationEntry OperationEntry { get; set; }
     
        #endregion Navigation Properties
    }
}
