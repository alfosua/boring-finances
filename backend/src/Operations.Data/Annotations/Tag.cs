using BoringFinances.Operations.Data.Accounts;
using BoringFinances.Operations.Data.FinancialUnits;
using BoringFinances.Operations.Data.Operations;
using System.ComponentModel.DataAnnotations;

namespace BoringFinances.Operations.Data.Annotations;

public class Tag
{
    public Tag()
    {
        Name = nameof(DefaultTextOptions.Unnamed);
        Accounts = null!;
        Operations = null!;
        FinancialUnits = null!;
    }

    [Key]
    public long TagId { get; set; }

    [Required, StringLength(64)]
    public string Name { get; set; }

    public ICollection<Account> Accounts { get; set; }

    public ICollection<Operation> Operations { get; set; }

    public ICollection<FinancialUnit> FinancialUnits { get; set; }
}
