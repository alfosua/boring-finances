namespace BoringFinances.Operations.WebApi.Accounts;

public interface IAccountFilter
{
    public string? Text { get; set; }

    public List<long>? Ids { get; set; }

    public List<string>? Tags { get; set; }
}
