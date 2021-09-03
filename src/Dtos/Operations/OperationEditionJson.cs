using System.Collections.Generic;

namespace BoringSoftware.Finances.Dtos.Operations
{
    public class OperationEditionJson
    {
        public IEnumerable<OperationEntryEditionJson> Entries { get; set; }

        public IEnumerable<string> Notes { get; set; }
    }
}
