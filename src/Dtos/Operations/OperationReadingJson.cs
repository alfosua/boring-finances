using System;
using System.Collections.Generic;

namespace BoringSoftware.Finances.Dtos.Operations
{
    public class OperationReadingJson
    {
        public Guid Id { get; set; }

        public IEnumerable<OperationEntryReadingJson> Entries { get; set; }

        public IEnumerable<string> Notes { get; set; }
    }
}
