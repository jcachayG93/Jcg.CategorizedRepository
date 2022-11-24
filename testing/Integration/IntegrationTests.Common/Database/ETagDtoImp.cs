using Support.UnitOfWork.Api;

namespace IntegrationTests.Common.Database;

internal class ETagDtoImp<T> : IETagDto<T>
{
    public ETagDtoImp(string etag, T payload)
    {
        Etag = etag;
        Payload = payload;
    }

    /// <inheritdoc />
    public string Etag { get; }

    /// <inheritdoc />
    public T Payload { get; }
}