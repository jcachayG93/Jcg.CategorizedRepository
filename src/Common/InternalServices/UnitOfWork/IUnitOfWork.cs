using Common.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.InternalServices.UnitOfWork
{
    /// <summary>
    /// A Unit of work that issolates operations from the database client.
    /// Has a cache so data is ready from the database only once per key. Any changes are reflected in the local data but not sent to the database until CommitChanges is called.
    /// ETags for each key are kept internally to be sent back to the database when chnages are commited.
    /// This is designed to be a scoped service (i.e. one instance per http request)
    /// </summary>
    /// <typeparam name="TAggregateDatabaseModel"></typeparam>
    /// <typeparam name="TLookupDatabaseModel"></typeparam>
    public interface IUnitOfWork<TAggregateDatabaseModel, TLookupDatabaseModel>
        where TAggregateDatabaseModel : class
        where TLookupDatabaseModel : class
    {
        /// <summary>
        /// Gets the category index
        /// </summary>
        /// <param name="categoryKey">The category key</param>
        /// <returns>The category index, null if it not initialized</returns>
        /// <exception cref="CategoryIndexIsUninitializedException">When the CategoryIndex is not found</exception>
        Task<CategoryIndex<TLookupDatabaseModel>> GetCategoryIndex(string categoryKey,
            CancellationToken cancellationToken);

        /// <summary>
        /// Upsert the category index
        /// </summary>
        Task UpsertCategoryIndex(string categoryKey,
            CategoryIndex<TLookupDatabaseModel> categoryIndex,
            CancellationToken cancellationToken);

        /// <summary>
        /// Commits all the changes. This operation can be called only once for the lifetime of this UnitOfWork
        /// </summary>
        /// <returns></returns>
        /// <exception cref="CommitMayBeCalledOnlyOnceException">When called more than once. Once changes are committed, you must use a different instance for the
        /// unit of work</exception>
        Task CommitChangesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Gets the aggregate for the matching key. 
        /// </summary>
        /// <param name="key">The aggregate key key</param>
        /// <returns>The aggregate, null if not found</returns>
        Task<TAggregateDatabaseModel?> GetAggregateAsync(string key,
            CancellationToken cancellationToken);

        /// <summary>
        /// Upserts the aggregate
        /// </summary>
        /// <param name="key"></param>
        /// <param name="aggregate"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UpsertAggregateAsync(string key, TAggregateDatabaseModel aggregate,
            CancellationToken cancellationToken);


    }
}
