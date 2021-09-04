using System;
using System.Collections.Generic;

namespace BoringSoftware.Finances.Core.Operations
{
    public class OperationCrudFacade
    {
        public Guid? Id { get; set; }

        public IEnumerable<OperationCrudEntryFacade> Entries { get; set; }

        public IEnumerable<string> Notes { get; set; }
    }
}
