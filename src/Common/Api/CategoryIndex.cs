using Common.Api;

namespace Support.UnitOfWork.Api
{
    /// <summary>
    ///     Represents an index table to lookup items items belonging to a category
    /// </summary>
    /// <typeparam name="TLookupDatabaseModel"></typeparam>
    public class CategoryIndex<TLookupDatabaseModel>
        where TLookupDatabaseModel : IRepositoryKey
    {
        public IEnumerable<TLookupDatabaseModel> Lookups { get; set; }
            = Array.Empty<TLookupDatabaseModel>();
    }
}