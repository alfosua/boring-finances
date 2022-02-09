using BoringFinances.Operations.Data;
using BoringFinances.Operations.WebApi.Utilities;

namespace BoringFinances.Operations.WebApi.FinancialUnits;

public class FinancialUnitDto
{
    public FinancialUnitDto()
    {
        Name = nameof(DefaultTextOptions.Untitled);
        Notes = Enumerable.Empty<string>();
        Tags = Array.Empty<string>();
    }

    public long FinancialUnitId { get; set; }

    public string Name { get; set; }

    public byte FinancialUnitTypeId { get; set; }

    public string? Code { get; set; }

    public int FractionalDigits { get; set; }

    public IEnumerable<string> Notes { get; set; }

    [TagNamesContainer]
    public ICollection<string> Tags { get; set; }
}
