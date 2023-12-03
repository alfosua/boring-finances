using System.Text.Json.Serialization;

namespace BoringFinances.Services.Contracts;

public class BudgetInput
{
    public required string Name { get; set; }
    public ICollection<BudgetItemInput> Items { get; set; } = Array.Empty<BudgetItemInput>();
}

public class BudgetItemInput
{
    public required string Description { get; set; }
    public AmountDefinitionInput Definition { get; set; } = new AmountDefinitionInput();
}

public class AmountDefinitionInput
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public decimal? FixedAmount { get; set; }
}

public class BudgetOutput
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public ICollection<BudgetItemInput> Items { get; set; } = Array.Empty<BudgetItemInput>();
}
