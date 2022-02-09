using System.ComponentModel.DataAnnotations;

namespace BoringFinances.Operations.Data.FinancialUnits;

public class FinancialUnitType
{
    public FinancialUnitType()
    {
        Code = nameof(FinancialUnitTypeOptions.None);
        FinancialUnits = null!;
    }

    [Key]
    public byte FinancialUnitTypeId { get; set; }

    [Required, StringLength(16)]
    public string Code { get; set; }

    public ICollection<FinancialUnit> FinancialUnits { get; set; }
}
