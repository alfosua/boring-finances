using BoringFinances.Operations.WebApi.Utilities;
using System.ComponentModel.DataAnnotations;

namespace BoringFinances.Operations.WebApi.Accounts;

public class AccountPagedFilter : IAccountFilter, IPagination
{
    public string? Text { get; set; }

    public List<long>? Ids { get; set; }

    public List<string>? Tags { get; set; }

    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; }

    [Range(1, int.MaxValue)]
    public int PageSize { get; set; }
}
