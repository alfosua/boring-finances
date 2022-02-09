using BoringFinances.Operations.Data;
using BoringFinances.Operations.WebApi.Utilities;

namespace BoringFinances.Operations.WebApi.Accounts;

public class AccountDto
{
    public AccountDto()
    {
        Title = nameof(DefaultTextOptions.Untitled);
        Notes = Enumerable.Empty<string>();
        Tags = Array.Empty<string>();
    }

    public long AccountId { get; set; }

    public string Title { get; set; }

    public byte AccountTypeId { get; set; }

    public string? Code { get; set; }

    public IEnumerable<string> Notes { get; set; }

    [TagNamesContainer]
    public ICollection<string> Tags { get; set; }
}
