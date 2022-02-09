namespace BoringFinances.Operations.WebApi.FinancialUnits;

public interface IFinancialUnitFilter
{
    public string? Text { get; set; }

    public List<long>? Ids { get; set; }

    public List<string>? Tags { get; set; }
}
