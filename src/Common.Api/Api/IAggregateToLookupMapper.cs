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
/// <typeparam name="TLookupModel">
///     This is the same model that the user will interact with when querying the category and
///     to be stored in the database
/// </typeparam>
/// <remarks>
///     In this library, the aggregate has two representations, one for the database and one for the client. That is
///     because a common use case is to store a ddd aggregate. For the lookup, that is not the case, so,
///     to keep the library as simple as possible, I decided to use the same model for the database and the client
/// </remarks>
public interface IAggregateToLookupMapper<TAggregateDatabaseModel,
    TLookupModel>
    where TAggregateDatabaseModel : class, new()
    where TLookupModel : class, new()
{
    TLookupModel ToLookup(TAggregateDatabaseModel aggregate);
}