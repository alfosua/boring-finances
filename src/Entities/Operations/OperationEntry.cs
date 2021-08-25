using BoringSoftware.Finances.Entities.Accounts;
using BoringSoftware.Finances.Entities.FinancialUnits;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BoringSoftware.Finances.Entities.Operations
{
    public class OperationEntry
    {
        #region Mapped Properties

        [Key]
        public Guid Id { get; set; }

        public Guid OperationId { get; set; }

        public Guid AccountId { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        public Guid FinancialUnitId { get; set; }

        public byte OperationEntryTypeId { get; set; }

        #endregion Mapped Properties

        #region Navigation Properties

        public Account Account { get; set; }

        public FinancialUnit FinancialUnit { get; set; }

        public Operation Operation { get; set; }

        public OperationEntryType Type { get; set; }

        public ICollection<OperationEntryNote> Notes { get; set; }

        #endregion Navigation Properties
    }
}