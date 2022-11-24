using Common.Api;

namespace Testing.Common.Types;

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
}