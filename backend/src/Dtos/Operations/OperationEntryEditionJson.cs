using System;
using System.Collections.Generic;

namespace BoringSoftware.Finances.Dtos.Operations
{
    public class OperationEntryEditionJson
    {
        public Guid? Id { get; set; }

        public DateTime DateTime { get; set; }

        public string Account { get; set; }

        public string FinancialUnit { get; set; }

        public decimal Amount { get; set; }

        public string OperationEntryType { get; set; }

        public IEnumerable<string> Notes { get; set; }
    }
}
