using BoringFinances.Operations.Data.Accounts;
using BoringFinances.Operations.Data.FinancialUnits;
using BoringFinances.Operations.Data.Operations;
using System.ComponentModel.DataAnnotations;

namespace BoringFinances.Operations.Data.Annotations;

public class Note
{
    public Note()
    {
        Text = string.Empty;
        Accounts = null!;
        Operations = null!;
        OperationEntries = null!;
        FinancialUnits = null!;
    }

    [Key]
    public long NoteId { get; set; }

    [Required, StringLength(64)]
    public string Text { get; set; }

    public ICollection<Account> Accounts { get; set; }

    public ICollection<Operation> Operations { get; set; }

    public ICollection<OperationEntry> OperationEntries { get; set; }

    public ICollection<FinancialUnit> FinancialUnits { get; set; }
}
