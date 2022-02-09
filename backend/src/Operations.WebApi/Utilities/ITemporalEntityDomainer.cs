using BoringFinances.Operations.Data;

namespace BoringFinances.Operations.WebApi.Utilities;

public interface ITemporalEntityDomainer
{
    TEntity TimestampCreationTo<TEntity>(TEntity entity, DateTime dateTime) where TEntity : ITimestampsCreation;

    TEntity TimestampDeletionTo<TEntity>(TEntity entity, DateTime dateTime) where TEntity : ITimestampsDeletion;

    TEntity TimestampEffectTo<TEntity, TVersion>(TEntity operationEntry, TVersion version, DateTime dateTime)
        where TEntity : ITemporalEntity<TVersion>
        where TVersion : ITimestampsEffect;
}
