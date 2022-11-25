namespace Jcg.CategorizedRepository.Api
{
    /// <summary>
    ///     Maps between the Aggregate (Which can be a well encapsulated class) and the database model (which should be
    ///     compatible with the database). Both represents the same entity but for different purposes.
    ///     Both models can evolve independently, in such a case you will need to update the implementation of this mapper.
    /// </summary>
    /// <typeparam name="TAggregate">
    ///     The aggregate. This can be anything, even a well encapsulated DDD Aggregate in which all
    ///     changes are done using methods. It can be a record, struct or class. There are no constraints.
    /// </typeparam>
    /// <typeparam name="TAggregateDatabaseModel">
    ///     A model that represents the aggregate but is designed to work with the chosen
    ///     database. Typically a class with a parameterless constructor and public automatic properties
    /// </typeparam>
    /// <remarks>
    ///     Because of this mapper, you can refactor the aggregate often while keeping the database model as fixed as
    ///     possible.
    /// </remarks>
    public interface IAggregateMapper<TAggregate, TAggregateDatabaseModel>
        where TAggregateDatabaseModel : class

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