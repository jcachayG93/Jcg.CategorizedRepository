﻿using Support.UnitOfWork.Api;

namespace Testing.Common.Types;

internal class AggregateETag : IETagDto<AggregateDatabaseModel>
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