using BoringFinances.Operations.Data.Annotations;
using BoringFinances.Operations.Data.Operations;
using System.ComponentModel.DataAnnotations;

namespace BoringFinances.Operations.Data.FinancialUnits;

public class FinancialUnit : ITimestampsCreation, ITimestampsDeletion, IHasTags, IHasNotes
{
    public FinancialUnit()
    {
        Name = nameof(DefaultTextOptions.Unnamed);
        FinancialUnitType = null!;
        OperationEntryVersions = null!;
        Tags = new List<Tag>();
        Notes = new List<Note>();
    }

    [Key]
    public long FinancialUnitId { get; set; }

    [Required]
    public string Name { get; set; }

    [StringLength(256)]
    public string? Code { get; set; }

    public byte FinancialUnitTypeId { get; set; }

    public int FractionalDigits { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Deleted { get; set; }

    public FinancialUnitType FinancialUnitType { get; set; }

    public ICollection<OperationEntryVersion> OperationEntryVersions { get; set; }

    public ICollection<Tag> Tags { get; set; }

    public ICollection<Note> Notes { get; set; }
}
