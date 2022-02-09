using BoringFinances.Operations.Data.Accounts;
using BoringFinances.Operations.Data.FinancialUnits;
using System.ComponentModel.DataAnnotations;

namespace BoringFinances.Operations.Data.Operations;

public class OperationEntryVersion : ITimestampsEffect
{
    public OperationEntryVersion()
    {
        Operation = null!;
        OperationEntry = null!;
        Account = null!;
        FinancialUnit = null!;
        OperationAction = null!;
    }

    [Key]
    public long OperationId { get; set; }

    [Key]
    public int OperationEntryId { get; set; }

    [Key]
    public DateTime Effective { get; set; }

    public long AccountId { get; set; }

    public long FinancialUnitId { get; set; }

    public byte OperationActionId { get; set; }

    public long Amount { get; set; }

    public DateTime DateTime { get; set; }

    public Operation Operation { get; set; }

    public OperationEntry OperationEntry { get; set; }

    public Account Account { get; set; }

    public FinancialUnit FinancialUnit { get; set; }

    public OperationAction OperationAction { get; set; }
}
