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
        }

        public Task<TAggregateDatabaseModel?> GetAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task UpsertAsync(string key, TAggregateDatabaseModel aggregate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UpsertedItem<TAggregateDatabaseModel>> UpsertedItems
        {
            get;
        }
    }
}