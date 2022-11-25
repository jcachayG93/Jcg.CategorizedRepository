using Jcg.CategorizedRepository.Api.Exceptions;

namespace Jcg.CategorizedRepository.Api
{
    /// <summary>
    ///     A repository that sits between the database client and your application to automate several features. Works like a
    ///     Unit of work, where changes are kept isolated in this instance until they are committed.
    ///     It also acts as a cache. Data is read from the database up to one time per key, further queries will return data
    ///     from the cache which reflect all local uncommited changes.
    /// </summary>
    /// <typeparam name="TAggregate">The detailed model of the aggregate, it can be a composition of more than one class</typeparam>
    /// <typeparam name="TLookup">
    ///     A short, lightweight representation of the aggregate. When querying items in the category, a
    ///     collection of lookups will be returned
    /// </typeparam>
    public interface ICategorizedRepository
        <TAggregate, TLookup>
        where TLookup : IRepositoryLookup
    {
        /// <summary>
        ///     Adds an operation that initializes a category so aggregates can be added to it.
        /// </summary>
        /// <exception cref="CategoryIndexIsAlreadyInitializedException">Thrown if the category is already initialized</exception>
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
        /// <remarks>
        ///     If the aggregate is DELETED, this method will RETURN IT ANYWAYS. This is a design choice.
        ///     To see which ones are deleted, use the Lookup methods below.
        /// </remarks>
        /// <param name="key">The key</param>
        /// <returns>The aggregate</returns>
        Task<TAggregate?> GetAggregateAsync(RepositoryIdentity key,
            CancellationToken cancellationToken);

        /// <summary>
        ///     Upserts the aggregate. Inserts if no aggregate matches the key,
        ///     replaces otherwise
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="aggregate">The aggregate to upsert</param>
        /// <exception cref="CategoryIndexIsUninitializedException">You cant add an aggregate when the category is uninitialized</exception>
        /// <exception cref="UnitOfWorkWasAlreadyCommittedException">
        ///     Thrown when this method is called after the unit of work was
        ///     already committed
        /// </exception>
        Task UpsertAsync(RepositoryIdentity key, TAggregate aggregate,
            CancellationToken cancellationToken);


        /// <summary>
        ///     Returns all the lookups for non-deleted aggregates that belong to the category.
        ///     Each result item has the payload as a property.
        /// </summary>
        /// <remarks>
        ///     This unit of work caches data so it is read from the database only once per key. The local cache reflects any
        ///     operations performed, but those changes are not applied to the database until commit is called.
        /// </remarks>
        /// <exception cref="CategoryIndexIsUninitializedException">Thrown when the category index is uninitialized</exception>
        Task<IEnumerable<LookupDto<TLookup>>> LookupNonDeletedAsync(
            CancellationToken cancellationToken);


        /// <summary>
        ///     Returns all the lookups for deleted aggregates that belong to the category
        ///     Each result item has the payload as a property.
        /// </summary>
        /// <remarks>
        ///     This unit of work caches data so it is read from the database only once per key. The local cache reflects any
        ///     operations performed, but those changes are not applied to the database until commit is called.
        /// </remarks>
        /// <exception cref="CategoryIndexIsUninitializedException">Thrown when the category index is uninitialized</exception>
        Task<IEnumerable<LookupDto<TLookup>>> LookupDeletedAsync(
            CancellationToken cancellationToken);

        /// <summary>
        ///     Performs a Soft delete operation. Moves the aggregate from the non-deleted to deleted
        ///     category index.
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="CategoryIndexIsUninitializedException">Thrown when the category index is uninitialized</exception>
        /// <exception cref="LookupNotFoundInCategoryIndexException">
        ///     Thrown if a lookup that match the key was not found between
        ///     the NON-DELETED ones
        /// </exception>
        /// <exception cref="UnitOfWorkWasAlreadyCommittedException">
        ///     Thrown when this method is called after the unit of work was
        ///     already committed
        /// </exception>
        Task DeleteAsync(RepositoryIdentity key,
            CancellationToken cancellationToken);

        /// <summary>
        ///     Restores a deleted aggregate
        /// </summary>
        /// <param name="key">The aggregate key</param>
        /// <exception cref="CategoryIndexIsUninitializedException">Thrown when the category index is uninitialized</exception>
        /// <exception cref="LookupNotFoundInCategoryIndexException">
        ///     Thrown if a lookup that match the key was not found between
        ///     the DELETED ones
        /// </exception>
        /// <exception cref="UnitOfWorkWasAlreadyCommittedException">
        ///     Thrown when this method is called after the unit of work was
        ///     already committed
        /// </exception>
        Task RestoreAsync(RepositoryIdentity key,
            CancellationToken cancellationToken);

        /// <summary>
        ///     Commits all the changes to the database. This method can be called once for the lifetime of this unit of work
        /// </summary>
        /// <exception cref="UnitOfWorkWasAlreadyCommittedException">Thrown when this method is called more than once</exception>
        Task CommitChangesAsync(CancellationToken cancellationToken);
    }
}