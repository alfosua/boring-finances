using BoringSoftware.Finances.Entities.Operations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BoringSoftware.Finances.Entities.FinancialUnits
{
    public class FinancialUnit
    {
        #region Mapped Properties

        [Key]
        public Guid Id { get; set; }

        public byte FinancialUnitTypeId { get; set; }

        [Required, StringLength(16)]
        public string Kebab { get; set; }

        #endregion Mapped Properties

        #region Navigation Properties

        public ICollection<OperationEntry> OperationEntries { get; set; }

        public FinancialUnitType Type { get; set; }

        #endregion Navigation Properties
    }
}
