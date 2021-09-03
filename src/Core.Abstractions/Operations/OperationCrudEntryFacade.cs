using System;
using System.Collections.Generic;

namespace BoringSoftware.Finances.Core.Operations
{
    public class OperationCrudEntryFacade
    {
        public Guid? Id { get; set; }

        public DateTime DateTime { get; set; }

        public decimal Amount { get; set; }

        public Guid FinancialUnitId { get; set; }

        public byte OperationEntryTypeId { get; set; }

        public Guid AccountId { get; set; }

        public IEnumerable<string> Notes { get; set; }
    }
}
