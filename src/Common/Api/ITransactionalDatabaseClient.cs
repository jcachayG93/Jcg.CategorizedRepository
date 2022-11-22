using Common.Api;

namespace Support.UnitOfWork.Api
{
    /// <summary>
    ///     A database client that  this library uses to talk to the client database
    /// </summary>
    /// <typeparam name="TAggregateDatabaseModel">The type representing the aggregate to be stored in the database</typeparam>
    /// <typeparam name="TLookupDatabaseModel">The type representing the aggregate lookup to be stored in the database</typeparam>
    public interface ITransactionalDatabaseClient
        <TAggregateDatabaseModel, TLookupDatabaseModel>
        where TAggregateDatabaseModel : class, IRepositoryKey
        where TLookupDatabaseModel : class, IRepositoryKey
    {
        /// <summary>
        ///     Gets the aggregate
        /// </summary>
        /// <param name="key">The aggregate key</param>
        /// <returns>A dto representing the data payload and its associated ETag value</returns>
        Task<IETagDto<TAggregateDatabaseModel>?> GetAggregateAsync(string key,
            CancellationToken cancellationToken);

        /// <summary>
        ///     Adds an Upsert operation to the list of transactions to be commited.
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="eTag">
        ///     The ETag value for the payload. Blank if this is an insert operation. Value returned by GetAggregate
        ///     if this is a replace operation.
        /// </param>
        /// <param name="aggregate">The aggregate</param>
        /// <remarks>
        ///     This method is responsible for checking that the eTag matches the expected value for the key, but that
        ///     exception will be thrown after commit
        /// </remarks>
        Task UpsertAggregateAsync(string key, string eTag,
            TAggregateDatabaseModel aggregate,
            CancellationToken cancellationToken);

        /// <summary>
        ///     Gets the Category index for the specified category
        /// </summary>
        /// <param name="categoryKey">This is just the key for the dataase model representing the category index</param>
        /// <returns>A dto representing the data payload and its associated ETag value</returns>
        Task<IETagDto<CategoryIndex<TLookupDatabaseModel>>?> GetCategoryIndex(
            string categoryKey,
            CancellationToken cancellationToken);

        /// <summary>
        ///     Adds an Upsert operation to the list of transactions to be commited.
        /// </summary>
        /// <param name="categoryKey">The key</param>
        /// <param name="eTag">
        ///     The ETag value for the payload. Blank if this is an insert operation. Value returned by GetAggregate
        ///     if this is a replace operation.
        /// </param>
        /// <param name="categoryIndex">The category index</param>
        /// <remarks>
        ///     This method is responsible for checking that the eTag matches the expected value for the key, but that
        ///     exception will be thrown after commit
        /// </remarks>
        Task UpsertCategoryIndex(string categoryKey,
            string eTag,
            CategoryIndex<TLookupDatabaseModel> categoryIndex,
            CancellationToken cancellationToken);

        /// <summary>
        ///     Commits all Upsert operations added in previous steps in a single transaction. If any ETag do not match the
        ///     expected vaue for the key,
        ///     discard this operation and throw an exception.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="Exception">
        ///     Throw any exception if the ETag value for any of the operations do not match the expected value. In such case,
        ///     discard all changes
        /// </exception>
        Task CommitTransactionAsync(CancellationToken cancellationToken);
    }
}