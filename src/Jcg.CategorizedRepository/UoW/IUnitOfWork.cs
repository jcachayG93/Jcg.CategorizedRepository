using Jcg.CategorizedRepository.Api;
using Jcg.CategorizedRepository.Api.Exceptions;

namespace Jcg.CategorizedRepository.UoW;

internal interface IUnitOfWork<TAggregateDatabaseModel,
    TLookup>
    where TAggregateDatabaseModel : class
{
    /// <summary>
    ///     Gets the non deleted items category index
    /// </summary>
    /// <param name="categoryKey">The category key</param>
    /// <returns>The category index</returns>
    /// <exception cref="CategoryIndexIsUninitializedException">When the CategoryIndex is not found</exception>
    Task<CategoryIndex<TLookup>>
        GetNonDeletedItemsCategoryIndex(
            CancellationToken cancellationToken);

    /// <summary>
    ///     Gets the deleted items category index
    /// </summary>
    /// <param name="categoryKey">The category key</param>
    /// <returns>The category index</returns>
    /// <exception cref="CategoryIndexIsUninitializedException">When the CategoryIndex is not found</exception>
    Task<CategoryIndex<TLookup>>
        GetDeletedItemsCategoryIndex(
            CancellationToken cancellationToken);

    /// <summary>
    ///     Checks if the deleted category index exists in the database
    /// </summary>
    Task<bool> CategoryIndexIsInitializedAsync(
        CancellationToken cancellationToken);

    /// <summary>
    ///     UpsertOLD the deleted items category index
    /// </summary>
    Task UpsertDeletedItemsCategoryIndex(
        CategoryIndex<TLookup> deletedItemsCategoryIndex,
        CancellationToken cancellationToken);

    /// <summary>
    ///     UpsertOLD the non-deleted items category index
    /// </summary>
    Task UpsertNonDeletedItemsCategoryIndex(
        CategoryIndex<TLookup> nonDeletedItemsCategoryIndex,
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
    Task UpsertAggregateAsync(
        string key,
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