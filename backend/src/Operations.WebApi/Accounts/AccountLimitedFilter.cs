using BoringFinances.Operations.WebApi.Utilities;
using System.ComponentModel.DataAnnotations;

namespace BoringFinances.Operations.WebApi.Accounts;

public class AccountLimitedFilter : IAccountFilter, ILimitation
{
    public string? Text { get; set; }

    public List<long>? Ids { get; set; }

    public List<string>? Tags { get; set; }

    [Range(1, int.MaxValue)]
    public int? Limit { get; set; }
        
    [Range(0, int.MaxValue)]
    public int? Offset { get; set; }
}
