namespace Jcg.DataAccessRepositories;

/// <summary>
///     Maps the lookup model that is stored in the database to the model the client will receive when querying the
///     repository
/// </summary>
/// <typeparam name="TLookupDatabaseModel">The Lookup database model, designed to be compatible with the chosen database</typeparam>
/// <typeparam name="TLookup">The lookup for the client. Can be any type and be encapsulated</typeparam>
public interface ILookupMapper<TLookupDatabaseModel, TLookup>
{
    TLookup Map(TLookupDatabaseModel databaseModel);
}