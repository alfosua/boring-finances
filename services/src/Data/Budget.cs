namespace BoringFinances.Services.Data;

public class Budget
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public ICollection<BudgetItem> Items { get; set; } = new List<BudgetItem>();
}

public class BudgetItem
{
    public required string Description { get; set; }
    public required IAmountDefinition Definition { get; set; }
}

public interface IAmountDefinition;

public interface IFixedAmountDefinition : IAmountDefinition
{
    decimal Amount { get; }
}

public class FixedAmountDefinition(decimal amount) : IFixedAmountDefinition
{
    public decimal Amount { get; set; } = amount;
}
