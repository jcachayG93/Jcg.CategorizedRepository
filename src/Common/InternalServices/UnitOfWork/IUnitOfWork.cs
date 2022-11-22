using Support.UnitOfWork.Api;
using Support.UnitOfWork.Api.Exceptions;

namespace Common.InternalServices.UnitOfWork
{
    /// <summary>
    ///     A Unit of work that issolates operations from the database client.
    ///     Has a cache so data is ready from the database only once per key. Any changes are reflected in the local data but
    ///     not sent to the database until CommitChanges is called.
    ///     ETags for each key are kept internally to be sent back to the database when chnages are commited.
    ///     This is designed to be a scoped service (i.e. one instance per http request)
    /// </summary>
    /// <typeparam name="TAggregateDatabaseModel"></typeparam>
    /// <typeparam name="TLookupDatabaseModel"></typeparam>
    internal interface IUnitOfWork<TAggregateDatabaseModel,
        TLookupDatabaseModel>
        where TAggregateDatabaseModel : class
        where TLookupDatabaseModel : class
    {
        /// <summary>
        ///     Gets the non deleted items category index
        /// </summary>
        /// <param name="categoryKey">The category key</param>
        /// <returns>The category index</returns>
        /// <exception cref="CategoryIndexIsUninitializedException">When the CategoryIndex is not found</exception>
        Task<CategoryIndex<TLookupDatabaseModel>>
            GetNonDeletedItemsCategoryIndex(
                CancellationToken cancellationToken);

        /// <summary>
        ///     Gets the deleted items category index
        /// </summary>
        /// <param name="categoryKey">The category key</param>
        /// <returns>The category index</returns>
        /// <exception cref="CategoryIndexIsUninitializedException">When the CategoryIndex is not found</exception>
        Task<CategoryIndex<TLookupDatabaseModel>>
            GetDeletedItemsCategoryIndex(
                CancellationToken cancellationToken);

        /// <summary>
        ///     Upsert the deleted items category index
        /// </summary>
        Task UpsertDeletedItemsCategoryIndex(
            CategoryIndex<TLookupDatabaseModel> deletedItemsCategoryIndex,
            CancellationToken cancellationToken);

        /// <summary>
        ///     Upsert the non-deleted items category index
        /// </summary>
        Task UpsertNonDeletedItemsCategoryIndex(
            CategoryIndex<TLookupDatabaseModel> nonDeletedItemsCategoryIndex,
            CancellationToken cancellationToken);

        /// <summary>
        ///     Gets the aggregate for the matching key.
        /// </summary>
        /// <param name="key">The aggregate key key</param>
        /// <returns>The aggregate, null if not found</returns>
        Task<TAggregateDatabaseModel?> GetAggregateAsync(string key,
            CancellationToken cancellationToken);

        /// <summary>
        ///     Upserts the aggregate
        /// </summary>
        /// <param name="key"></param>
        /// <param name="aggregate"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UpsertAggregateAsync(string key,
            TAggregateDatabaseModel aggregate,
            CancellationToken cancellationToken);

        /// <summary>
        ///     Commits all the changes. This operation can be called only once for the lifetime of this UnitOfWork
        /// </summary>
        /// <returns></returns>
        /// <exception cref="UnitOfWorkWasAlreadyCommittedException">
        ///     Thrown when called more than once. This unit of work can be committed up to one time only.
        /// </exception>
        Task CommitChangesAsync(
            CancellationToken cancellationToken);
    }
}