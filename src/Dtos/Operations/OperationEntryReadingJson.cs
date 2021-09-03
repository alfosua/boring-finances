using BoringSoftware.Finances.Dtos.Utils;
using System;
using System.Collections.Generic;

namespace BoringSoftware.Finances.Dtos.Operations
{
    public class OperationEntryReadingJson
    {
        public Guid Id { get; set; }

        public DateTime DateTime { get; set; }

        public decimal Amount { get; set; }

        public IdHrefPair<Guid> Account { get; set; }

        public IdHrefPair<Guid> FinancialUnit { get; set; }

        public IdCodeHrefGroup<byte> OperationEntryType { get; set; }

        public IEnumerable<string> Notes { get; set; }
    }
}
