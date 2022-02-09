using BoringFinances.Operations.WebApi.Utilities;

namespace BoringFinances.Operations.WebApi.Operations;

public class OperationDto
{
    public OperationDto()
    {
        Entries = Enumerable.Empty<OperationEntryDto>();
        Notes = Enumerable.Empty<string>();
        Tags = Array.Empty<string>();
    }

    public long OperationId { get; set; }

    public IEnumerable<OperationEntryDto> Entries { get; set; }

    public IEnumerable<string> Notes { get; set; }

    [TagNamesContainer]
    public ICollection<string> Tags { get; set; }
}
