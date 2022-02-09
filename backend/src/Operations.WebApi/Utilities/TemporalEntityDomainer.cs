using BoringFinances.Operations.Data;

namespace BoringFinances.Operations.WebApi.Utilities;

public class TemporalEntityDomainer : ITemporalEntityDomainer
{
    public TEntity TimestampEffectTo<TEntity, TVersion>(TEntity operationEntry, TVersion version, DateTime dateTime)
        where TEntity : ITemporalEntity<TVersion>
        where TVersion : ITimestampsEffect
    {
        version.Effective = dateTime;

        return operationEntry;
    }

    public TEntity TimestampCreationTo<TEntity>(TEntity entity, DateTime dateTime)
        where TEntity : ITimestampsCreation
    {
        entity.Created = dateTime;

        return entity;
    }

    public TEntity TimestampDeletionTo<TEntity>(TEntity entity, DateTime dateTime)
        where TEntity : ITimestampsDeletion
    {
        entity.Deleted = dateTime;

        return entity;
    }
}
