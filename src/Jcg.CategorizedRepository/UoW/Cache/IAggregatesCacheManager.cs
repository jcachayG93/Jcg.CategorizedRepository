using Jcg.CategorizedRepository.Api;

namespace Jcg.CategorizedRepository.UoW.Cache
{
    internal interface IAggregatesCacheManager<TAggregateDatabaseModel>
        where TAggregateDatabaseModel : class, IAggregateDataModel
    {
        /// <summary>
        ///     All the aggregates that were upserted in the local cache. The data includes the ETag
        ///     that was received when the items were read from the database. If the item was inserted then the ETag will be a
        ///     blank string.
        /// </summary>
        IEnumerable<UpsertedItem<TAggregateDatabaseModel>> UpsertedItems
        {
            get;
        }

        /// <summary>
        ///     Gets the data from the cache, if the key was not read before, gets the data from the database and adds it to the
        ///     cache.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Null if the data for the key does not exist in the database</returns>
        Task<TAggregateDatabaseModel?> GetAsync(string key);


        /// <summary>
        ///     Upserts the data in the local cache. If the key was not read before will read it from the database and add it to
        ///     the cache.
        ///     Adds the data to the UpsertedItems collection.
        /// </summary>
        /// <param name="key">The aggregate key</param>
        /// <param name="aggregate">The aggregate</param>
        Task UpsertAsync(string key, TAggregateDatabaseModel aggregate);
    }
}