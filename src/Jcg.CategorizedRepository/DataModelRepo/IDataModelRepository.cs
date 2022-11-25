using Jcg.CategorizedRepository.Api;
using Jcg.CategorizedRepository.Api.Exceptions;

namespace Jcg.CategorizedRepository.DataModelRepo
{
    // TODO: Rename to TLookup
    internal interface IDataModelRepository
        <TAggregateDatabaseModel, TLookupDatabaseModel>
        where TAggregateDatabaseModel : class, IAggregateDataModel
        where TLookupDatabaseModel : IRepositoryLookup
    {
        /// <summary>
        ///     Initializes the category indexes for the category
        /// </summary>
        Task InitializeCategoryIndexes(CancellationToken cancellationToken);

        /// <summary>
        ///     Gets the aggregate
        /// </summary>
        /// <param name="key">The aggregate key</param>
        /// <returns>The aggregate. Null if not found</returns>
        /// <exception cref="CategoryIndexIsUninitializedException">When the CategoryIndex is not found</exception>
        Task<TAggregateDatabaseModel?> GetAggregateAsync(Guid key,
            CancellationToken cancellationToken);

        /// <summary>
        ///     Gets the non-deleted aggregate lookups
        /// </summary>
        /// <returns>The category index, which contains all the lookup for those aggregates that are not deleted</returns>
        /// <exception cref="CategoryIndexIsUninitializedException">When the CategoryIndex is not found</exception>
        Task<CategoryIndex<TLookupDatabaseModel>> LookupNonDeletedAsync(
            CancellationToken cancellationToken);

        /// <summary>
        ///     Gets the deleted aggregate lookups
        /// </summary>
        /// <returns>
        ///     The category index, which contains all the lookup for those aggregates
        ///     that are deleted
        /// </returns>
        /// <exception cref="CategoryIndexIsUninitializedException">When the CategoryIndex is not found</exception>
        Task<CategoryIndex<TLookupDatabaseModel>> LookupDeletedAsync(
            CancellationToken cancellationToken);

        /// <summary>
        ///     Upsers the aggregate.
        /// </summary>
        /// <param name="aggregate">The aggregate</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="CategoryIndexIsUninitializedException">
        ///     When the CategoryIndex
        ///     is not found
        /// </exception>
        Task UpsertAsync(TAggregateDatabaseModel aggregate,
            CancellationToken cancellationToken);

        /// <summary>
        ///     Moves the lookup from the Non-Deleted to the Deleted category index
        /// </summary>
        /// <param name="key">The aggregate key</param>
        /// <exception cref="CategoryIndexIsUninitializedException">
        ///     When the CategoryIndex
        ///     is not found
        /// </exception>
        Task DeleteAsync(Guid key, CancellationToken cancellationToken);

        /// <summary>
        ///     Moves the lookup from the Deleted to the Non-Deleted category index
        /// </summary>
        /// <param name="key">The aggregate key</param>
        /// <exception cref="CategoryIndexIsUninitializedException">
        ///     When the CategoryIndex
        ///     is not found
        /// </exception>
        Task RestoreAsync(Guid key, CancellationToken cancellationToken);

        /// <summary>
        ///     Commits the changes to the database
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <exception cref="UnitOfWorkWasAlreadyCommittedException">
        Task CommitChangesAsync(CancellationToken cancellationToken);
    }
}