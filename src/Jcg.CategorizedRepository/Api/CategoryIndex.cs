namespace Jcg.CategorizedRepository.Api
{
    /// <summary>
    ///     A model that contains the lookups for the aggregates in the category. These can be the deleted or the non-deleted
    ///     ones depending on which method was used for the query
    /// </summary>
    /// <typeparam name="TLookupDatabaseModel">A light-weight and database friendly model of the lookup</typeparam>
    public class CategoryIndex<TLookupDatabaseModel>
    {
        public LookupDto<TLookupDatabaseModel>[] Lookups { get; set; }
            = Array.Empty<LookupDto<TLookupDatabaseModel>>();
    }
}