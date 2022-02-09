namespace BoringFinances.Operations.Data;

public interface ITemporalEntity<TVersion> : ITimestampsCreation
    , ITimestampsDeletion
    , IVersionedBy<TVersion>
    where TVersion : ITimestampsEffect
{
}

public interface IVersionedBy<TVersion> where TVersion : ITimestampsEffect
{
    public ICollection<TVersion> Versions { get; set; }
}
