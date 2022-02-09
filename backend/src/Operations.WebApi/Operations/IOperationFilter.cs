namespace BoringFinances.Operations.WebApi.Operations;

public interface IOperationFilter
{
    public string? Text { get; set; }

    public List<long>? Ids { get; set; }

    public List<string>? Tags { get; set; }
}
