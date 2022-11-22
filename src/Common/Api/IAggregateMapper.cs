namespace Common.Api.Api
{
    /// <summary>
    ///     One of this library features is that the client can easitly store and retrieve any type of entity (even a well
    ///     encapsulated domain drive design aggregate), for this to work, this library needs to be able to
    ///     map this aggregate to a database friendly model which represents the same information but is designed to work well
    ///     with the chosen database (for example, a class, with parameterless constructor, automatic public properties, etc).
    ///     This mapper does that mapping.
    /// </summary>
    /// <typeparam name="TAggregate">
    ///     The aggregate. This can be anything, even a well encapsulated DDD Aggregate in which all
    ///     changes must be done using methods. It can be an interface, record, struct or class.
    /// </typeparam>
    /// <typeparam name="TAggregateDatabaseModel">
    ///     A model that represents the aggregate but is designed to work with the chosen
    ///     database. Typically a class with a parameterless constructor and public automatic properties
    /// </typeparam>
    public interface IAggregateMapper<TAggregate, TAggregateDatabaseModel>
        where TAggregateDatabaseModel : class, new()
    {
        /// <summary>
        ///     Maps the database model to the aggregate model
        /// </summary>
        /// <param name="databaseModel">The database model representing the aggregate</param>
        /// <returns>The aggregate</returns>
        TAggregate ToAggregate(TAggregateDatabaseModel databaseModel);

        /// <summary>
        ///     Maps the aggregate to the database model
        /// </summary>
        /// <param name="aggregate">The aggregate</param>
        /// <returns>The database model representing the aggregate</returns>
        TAggregateDatabaseModel ToDatabaseModel(TAggregate aggregate);
    }
}