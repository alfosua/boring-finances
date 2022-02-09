using System.Text.Json.Serialization;

namespace BoringFinances.Operations.WebApi.Operations;

public class OperationEntryDto
{
    public OperationEntryDto()
    {
        Notes = Enumerable.Empty<string>();
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? OperationId { get; set; }

    public int? OperationEntryId { get; set; }

    public long AccountId { get; set; }

    public long FinancialUnitId { get; set; }

    public byte OperationActionId { get; set; }

    public long Amount { get; set; }

    public DateTime DateTime { get; set; }

    public IEnumerable<string> Notes { get; set; }
}
