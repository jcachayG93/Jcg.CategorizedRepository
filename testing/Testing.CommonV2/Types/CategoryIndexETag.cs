using Jcg.CategorizedRepository.Api;

namespace Testing.CommonV2.Types;

public class CategoryIndexETag : IETagDto<CategoryIndex<Lookup>>
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
}