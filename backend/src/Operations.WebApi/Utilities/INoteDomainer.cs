using BoringFinances.Operations.Data;
using BoringFinances.Operations.Data.Annotations;

namespace BoringFinances.Operations.WebApi.Utilities;

public interface INoteDomainer
{
    Note DecorateNoteWithText(Note note, string text);

    TEntity DecorateWithNotesTo<TEntity>(TEntity entity, IEnumerable<string> noteTexts) where TEntity : IHasNotes;

    List<Note> MergeNotesWithNewTexts(IEnumerable<Note> notes, IEnumerable<string> texts);
}
