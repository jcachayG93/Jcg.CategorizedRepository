using Jcg.Repositories.Api;
using Jcg.Repositories.Api.Exceptions;

namespace Support.DataModelRepository.Strategies
{
    internal interface IQueryStrategy<TAggregateDatabaseModel,
        TLookupDatabaseModel>
        where TAggregateDatabaseModel : class, IAggregateDataModel
        where TLookupDatabaseModel : ILookupDataModel
    {
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
    }
}