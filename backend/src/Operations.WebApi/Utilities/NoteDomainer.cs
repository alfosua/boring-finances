using BoringFinances.Operations.Data;
using BoringFinances.Operations.Data.Annotations;
using Microsoft.EntityFrameworkCore;

namespace BoringFinances.Operations.WebApi.Utilities;

public class NoteDomainer : INoteDomainer
{
    public TEntity DecorateWithNotesTo<TEntity>(TEntity entity, IEnumerable<string> noteTexts)
        where TEntity : IHasNotes
    {
        var willHaveNotes = noteTexts.TryGetNonEnumeratedCount(out var notesCountToBeAdded);

        if (willHaveNotes)
        {
            var notesCountDiff = notesCountToBeAdded - entity.Notes.Count;

            var targetNotes = notesCountDiff > 0
                ? entity.Notes.Concat(Enumerable.Repeat(new Note(), notesCountDiff))
                : notesCountDiff < 0
                ? entity.Notes.Take(notesCountToBeAdded)
                : entity.Notes;

            entity.Notes = MergeNotesWithNewTexts(targetNotes, noteTexts);
        }
        else
        {
            entity.Notes.Clear();
        }

        return entity;
    }

    public List<Note> MergeNotesWithNewTexts(IEnumerable<Note> notes, IEnumerable<string> texts)
    {
        return notes.Zip(texts)
            .Select(x => new { TargetNote = x.First, NewText = x.Second })
            .Select(x => DecorateNoteWithText(x.TargetNote, x.NewText))
            .ToList();
    }

    public Note DecorateNoteWithText(Note note, string text)
    {
        note.Text = text;
        return note;
    }
}
