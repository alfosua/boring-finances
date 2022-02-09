using BoringFinances.Operations.Data.Annotations;

namespace BoringFinances.Operations.Data;

public interface IHasNotes
{
    public ICollection<Note> Notes { get; set; }
}
