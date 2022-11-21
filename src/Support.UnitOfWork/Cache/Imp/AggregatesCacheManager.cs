using Support.UnitOfWork.Api;

namespace Support.UnitOfWork.Cache.Imp
{
    internal class AggregatesCacheManager<TAggregateDatabaseModel,
            TLookupDatabaseModel>
        : IAggregatesCacheManager<TAggregateDatabaseModel>
        where TAggregateDatabaseModel : class
        where TLookupDatabaseModel : class
    {
        public AggregatesCacheManager(
            ITransactionalDatabaseClient<TAggregateDatabaseModel,
                TLookupDatabaseModel> dbClient,
            Cache<TAggregateDatabaseModel> aggregatesCache)
        {
            _dbClient = dbClient;
            _aggregatesCache = aggregatesCache;
        }

        public async Task<TAggregateDatabaseModel?> GetAsync(string key)
        {
            await ReadAndAddToCacheIfNeededAsync(key);

            return _aggregatesCache.Get(key);
        }

        public async Task UpsertAsync(string key,
            TAggregateDatabaseModel aggregate)
        {
            await ReadAndAddToCacheIfNeededAsync(key);

            _aggregatesCache.Upsert(key, aggregate);
        }

        public IEnumerable<UpsertedItem<TAggregateDatabaseModel>>
            UpsertedItems => _aggregatesCache.UpsertedItems;

        private async Task ReadAndAddToCacheIfNeededAsync(string key)
        {
            if (!_aggregatesCache.HasKey(key))
            {
                var data =
                    await _dbClient.GetAggregateAsync(key,
                        CancellationToken.None);

                _aggregatesCache.Add(key, data);
            }
        }

        private readonly Cache<TAggregateDatabaseModel> _aggregatesCache;

        private readonly
            ITransactionalDatabaseClient<TAggregateDatabaseModel,
                TLookupDatabaseModel> _dbClient;
    }
}