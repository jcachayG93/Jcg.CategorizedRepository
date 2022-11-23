using Common.Api.Exceptions;

namespace Common.Api
{
    public interface ICategorizedRepository
        <TAggregate, TLookup>
    {
        /// <summary>
        ///     Adds an operation that initializes a category so aggregates can be added to it.
        /// </summary>
        /// <remarks>This operation is reflected inmediatelly but is not applied to the database until commit is called</remarks>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="CategoryIndexIsAlreadyInitializedException">Thrown if the category already exists</exception>
        /// <exception cref="UnitOfWorkWasAlreadyCommittedException">
        ///     Thrown when this method is called after the unit of work was
        ///     already committed
        /// </exception>
        Task InitializeCategoryIndexAsync(CancellationToken cancellationToken);

        /// <summary>
        ///     Gets the aggregate associated with the key
        /// </summary>
        /// <remarks>
        ///     This unit of work caches data so it is read from the database only once per key. The local cache reflects any
        ///     operations performed, but those changes are not applied to the database until commit is called.
        /// </remarks>
        /// <param name="key">The key</param>
        /// <returns>The aggregate</returns>
        Task<TAggregate?> GetAggregateAsync(RepositoryIdentity key,
            CancellationToken cancellationToken);

        /// <summary>
        ///     Upserts the aggregate. Inserts if no aggregate matches the key,
        ///     replaces otherwise
        /// </summary>
        /// <remarks>This operation is reflected inmediatelly but is not applied to the database until commit is called</remarks>
        /// <param name="key">The key</param>
        /// <param name="aggregate">The aggregate to upsert</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="CategoryIndexIsUninitializedException">When the CategoryIndex is not initialized (does not exist)</exception>
        /// <returns></returns>
        /// <exception cref="UnitOfWorkWasAlreadyCommittedException">
        ///     Thrown when this method is called after the unit of work was
        ///     already committed
        /// </exception>
        Task UpsertAsync(RepositoryIdentity key, TAggregate aggregate,
            CancellationToken cancellationToken);

        /// <summary>
        ///     Returns all the lookups that belong to the category
        /// </summary>
        /// <remarks>
        ///     This unit of work caches data so it is read from the database only once per key. The local cache reflects any
        ///     operations performed, but those changes are not applied to the database until commit is called.
        /// </remarks>
        /// <exception cref="CategoryIndexIsUninitializedException">When the CategoryIndex is not initialized (does not exist)</exception>
        Task<IEnumerable<TLookup>> LookupAsync(
            CancellationToken cancellationToken);

        /// <summary>
        ///     Returns all the lookups that belong to the category and are deleted.
        /// </summary>
        /// <remarks>
        ///     This unit of work caches data so it is read from the database only once per key. The local cache reflects any
        ///     operations performed, but those changes are not applied to the database until commit is called.
        /// </remarks>
        /// <param name="cancellationToken"></param>
        /// <exception cref="CategoryIndexIsUninitializedException">When the CategoryIndex is not initialized (does not exist)</exception>
        Task<IEnumerable<TLookup>> LookupDeletedAsync(
            CancellationToken cancellationToken);

        /// <summary>
        ///     Soft delete. Moves the aggregate from the non-deleted to deleted
        ///     category index.
        /// </summary>
        /// <remarks>This operation is reflected inmediatelly but is not applied to the database until commit is called</remarks>
        /// <param name="key">The key</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="CategoryIndexIsUninitializedException">When the CategoryIndex is not initialized (does not exist)</exception>
        /// <exception cref="LookupNotFoundInCategoryIndexException">If no non-deleted lookup matches the key</exception>
        /// <exception cref="UnitOfWorkWasAlreadyCommittedException">
        ///     Thrown when this method is called after the unit of work was
        ///     already committed
        /// </exception>
        Task DeleteAsync(RepositoryIdentity key,
            CancellationToken cancellationToken);

        /// <summary>
        ///     Restores a deleted aggregate
        /// </summary>
        /// <remarks>This operation is reflected inmediatelly but is not applied to the database until commit is called</remarks>
        /// <param name="key">The aggregate key</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="CategoryIndexIsUninitializedException">When the CategoryIndex is not initialized (does not exist)</exception>
        /// <exception cref="LookupNotFoundInCategoryIndexException">If no deleted lookup matches the key</exception>
        /// <exception cref="UnitOfWorkWasAlreadyCommittedException">
        ///     Thrown when this method is called after the unit of work was
        ///     already committed
        /// </exception>
        Task RestoreAsync(RepositoryIdentity key,
            CancellationToken cancellationToken);

        /// <summary>
        ///     Commits all the changes to the database. This method can be called once for the lifetime of this unit of work
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="UnitOfWorkWasAlreadyCommittedException">Thrown when this method is called more than once</exception>
        Task CommitChangesAsync(CancellationToken cancellationToken);
    }
}