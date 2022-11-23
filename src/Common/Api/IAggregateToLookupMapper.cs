namespace Common.Api.Api;

/// <summary>
///     One of this library features is that the client can query all aggregates belonging to a category (deleted or
///     non-deleted ones), and receive a collection of those lookups. For this to work, this library
///     needs to know how to map between the aggregate database model and the lookup, so, when upserting an aggregate, it
///     can map the lookup and upserted in the index table.
/// </summary>
/// <typeparam name="TAggregateDatabaseModel">
///     A model that represents the aggregate but is designed to work with the chosen
///     database. Typically a class with a parameterless constructor and public automatic properties
/// </typeparam>
/// <typeparam name="TLookupDatabaseModel">
///     This is the same model that the user will interact with when querying the category and
///     to be stored in the database
/// </typeparam>

public interface IAggregateToLookupMapper<TAggregateDatabaseModel,
    TLookupDatabaseModel>
    where TAggregateDatabaseModel : class, IAggregateDataModel
    where TLookupDatabaseModel : ILookupDataModel
{
    TLookupDatabaseModel ToLookup(TAggregateDatabaseModel aggregate);
}