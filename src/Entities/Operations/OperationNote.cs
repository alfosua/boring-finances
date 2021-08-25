using System;
using System.ComponentModel.DataAnnotations;

namespace BoringSoftware.Finances.Entities.Operations
{
    public class OperationNote
    {
        #region Mapped Properties

        [Key]
        public Guid Id { get; set; }

        public Guid OperationId { get; set; }

        [Required]
        public string Content { get; set; }

        #endregion Mapped Properties

        #region Navigation Properties

        public Operation Operation { get; set; }

        #endregion Navigation Properties
    }
}
