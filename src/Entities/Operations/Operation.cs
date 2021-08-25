using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BoringSoftware.Finances.Entities.Operations
{
    public class Operation
    {
        #region Mapped Properties

        [Key]
        public Guid Id { get; set; }

        #endregion Mapped Properties

        #region Navigation Properties

        public ICollection<OperationNote> Notes { get; set; }

        public ICollection<OperationEntry> Entries { get; set; }
     
        #endregion Navigation Properties
    }
}
