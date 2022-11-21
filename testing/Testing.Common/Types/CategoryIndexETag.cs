using Support.UnitOfWork.Api;

namespace Testing.Common.Types;

internal class CategoryIndexETag : IETagDto<CategoryIndex<LookupDatabaseModel>>
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
}