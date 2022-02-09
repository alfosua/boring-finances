using BoringFinances.Operations.Data;

namespace BoringFinances.Operations.WebApi.Utilities;

public interface ITagDomainer
{
    Task<TEntity> DecorateWithTagsToAsync<TEntity>(TEntity entity, IEnumerable<string> tagNames) where TEntity : IHasTags;
}
