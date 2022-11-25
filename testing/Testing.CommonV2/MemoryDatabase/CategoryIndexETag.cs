using Jcg.CategorizedRepository.Api;
using Testing.CommonV2.Types;

namespace Testing.CommonV2.MemoryDatabase;

public class
    CategoryIndexETag : IETagDto<CategoryIndex<Lookup>>
{
    public CategoryIndexETag(string etag,
        CategoryIndex<Lookup> payload)
    {
        Etag = etag;
        Payload = payload;
    }

    /// <inheritdoc />
    public string Etag { get; }

    /// <inheritdoc />
    public CategoryIndex<Lookup> Payload { get; }

    public CategoryIndexETag Clone()
    {
        return new(Etag, Payload);
    }

    public CategoryIndexETag CloneWithNewETag()
    {
        return new(RandomString(), Payload);
    }
}