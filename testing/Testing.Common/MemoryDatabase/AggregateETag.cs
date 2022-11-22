using Support.UnitOfWork.Api;
using Testing.Common.Types;

namespace Testing.Common.Doubles;

public class AggregateETag : IETagDto<AggregateDatabaseModel>
{
    public AggregateETag(string etag, AggregateDatabaseModel payload)
    {
        Etag = etag;
        Payload = payload;
    }

    /// <inheritdoc />
    public string Etag { get; }

    /// <inheritdoc />
    public AggregateDatabaseModel Payload { get; }

    public AggregateETag Clone()
    {
        return new(Etag, Payload);
    }

    public AggregateETag CloneWithNewETag()
    {
        return new(RandomString(), Payload);
    }
}