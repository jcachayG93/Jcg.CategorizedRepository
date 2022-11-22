using Support.UnitOfWork.Api;
using Support.UnitOfWork.Cache;

namespace Support.UnitOfWork
{
    internal class UnitOfWorkFactory
    {
        public IUnitOfWorkImp<TAggregateDatabaseModel, TLookupDatabaseModel>
            Create<TAggregateDatabaseModel, TLookupDatabaseModel>(
                ITransactionalDatabaseClient<TAggregateDatabaseModel,
                    TLookupDatabaseModel> dbClient,
                IAggregatesCacheManager<TAggregateDatabaseModel>
                    aggregatesCache,
                ICategoryIndexCacheManager<TLookupDatabaseModel>
                    deletedItemsCategoryIndexCache,
                ICategoryIndexCacheManager<TLookupDatabaseModel>
                    nonDeletedItemsCategoryIndexCache)
            where TLookupDatabaseModel : class
            where TAggregateDatabaseModel : class

        {
            return new UnitOfWorkImp<TAggregateDatabaseModel,
                TLookupDatabaseModel>(
                dbClient,
                aggregatesCache,
                deletedItemsCategoryIndexCache,
                nonDeletedItemsCategoryIndexCache);
        }
    }
}