using BoringFinances.Operations.Data.Annotations;
using BoringFinances.Operations.Data.Operations;
using System.ComponentModel.DataAnnotations;

namespace BoringFinances.Operations.Data.Accounts;

public class Account : ITimestampsCreation, ITimestampsDeletion, IHasTags, IHasNotes
{
    public Account()
    {
        Title = nameof(DefaultTextOptions.Untitled);
        AccountType = null!;
        OperationEntryVersions = null!;
        Tags = new List<Tag>();
        Notes = new List<Note>();
    }

    [Key]
    public long AccountId { get; set; }

    [Required]
    public string Title { get; set; }

    [StringLength(256)]
    public string? Code { get; set; }

    public byte AccountTypeId { get; set; }

    public AccountType AccountType { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Deleted { get; set; }

    public ICollection<OperationEntryVersion> OperationEntryVersions { get; set; }

    public ICollection<Tag> Tags { get; set; }

    public ICollection<Note> Notes { get; set; }
}
