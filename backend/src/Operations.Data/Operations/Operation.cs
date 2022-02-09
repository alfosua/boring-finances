using BoringFinances.Operations.Data.Annotations;
using System.ComponentModel.DataAnnotations;

namespace BoringFinances.Operations.Data.Operations;

public class Operation : ITimestampsCreation, ITimestampsDeletion, IHasNotes, IHasTags
{
    public Operation()
    {
        Entries = new List<OperationEntry>();
        Tags = new List<Tag>();
        Notes = new List<Note>();
    }

    [Key]
    public long OperationId { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Deleted { get; set; }

    public ICollection<OperationEntry> Entries { get; set; }

    public ICollection<Tag> Tags { get; set; }

    public ICollection<Note> Notes { get; set; }
}
