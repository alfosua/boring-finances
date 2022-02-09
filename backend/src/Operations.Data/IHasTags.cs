using BoringFinances.Operations.Data.Annotations;

namespace BoringFinances.Operations.Data;

public interface IHasTags
{
    public ICollection<Tag> Tags { get; set; }
}
