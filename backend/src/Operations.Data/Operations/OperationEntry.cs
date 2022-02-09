using BoringFinances.Operations.Data.Annotations;
using System.ComponentModel.DataAnnotations;

namespace BoringFinances.Operations.Data.Operations;

public class OperationEntry : ITemporalEntity<OperationEntryVersion>, IHasNotes
{
    public OperationEntry()
    {
        Operation = null!;
        Versions = new List<OperationEntryVersion> { new() };
        Notes = new List<Note>();
    }

    [Key]
    public long OperationId { get; set; }

    [Key]
    public int OperationEntryId { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Deleted { get; set; }

    public Operation Operation { get; set; }

    public ICollection<OperationEntryVersion> Versions { get; set; }

    public ICollection<Note> Notes { get; set; }
}
