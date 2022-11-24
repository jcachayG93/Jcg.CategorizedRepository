using Common.Api;
using Testing.Common.Types;

namespace Testing.Common.MemoryDatabase;

public class
    CategoryIndexETag : IETagDto<CategoryIndex<LookupDatabaseModel>>
{
    public CategoryIndexETag(string etag,
        CategoryIndex<LookupDatabaseModel> payload)
    {
        Etag = etag;
        Payload = payload;
    }

    /// <inheritdoc />
    public string Etag { get; }

    /// <inheritdoc />
    public CategoryIndex<LookupDatabaseModel> Payload { get; }

    public CategoryIndexETag Clone()
    {
        return new(Etag, Payload);
    }

    public CategoryIndexETag CloneWithNewETag()
    {
        return new(RandomString(), Payload);
    }
}