namespace Jcg.CategorizedRepository.Api;

/// <summary>
///     Maps the aggregate to a light weight dto (lookup database model) so this library can store a category index
///     containing all the lookups in the category.
/// </summary>
/// <typeparam name="TAggregateDatabaseModel">
///     The database model reprenting the full aggregate.
/// </typeparam>
/// <typeparam name="TLookupDatabaseModel">
///     A light weight model representing the minimal data the user would like to see when querying the items in the
///     category. Like an overview of an aggregate.
/// </typeparam>
public interface IAggregateToLookupMapper<TAggregateDatabaseModel,
    TLookupDatabaseModel>
    where TAggregateDatabaseModel : class, IAggregateDataModel
{
    /// <summary>
    ///     Maps the aggregate to the lookup
    /// </summary>
    TLookupDatabaseModel ToLookup(TAggregateDatabaseModel aggregate);
}