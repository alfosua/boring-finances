using System.ComponentModel.DataAnnotations;

namespace BoringFinances.Operations.Data.Operations;

public class OperationAction
{
    public OperationAction()
    {
        Code = nameof(OperationActionOptions.None);
        OperationEntryVersions = null!;
    }

    [Key]
    public byte OperationActionId { get; set; }

    [Required, StringLength(6)]
    public string Code { get; set; }

    public ICollection<OperationEntryVersion> OperationEntryVersions { get; set; }
}
