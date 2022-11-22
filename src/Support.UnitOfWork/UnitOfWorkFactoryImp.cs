using Support.UnitOfWork.Api;
using Support.UnitOfWork.Cache;
using Support.UnitOfWork.Cache.Imp;

namespace Support.UnitOfWork
{
    internal class UnitOfWorkFactoryImp
    {
        public IUnitOfWorkImp<TAggregateDatabaseModel, TLookupDatabaseModel>
            Create<TAggregateDatabaseModel, TLookupDatabaseModel>(
                string nonDeletedCategoryIndexKey,
                string deletedCategoryIndexKey,
                ITransactionalDatabaseClient<TAggregateDatabaseModel,
                    TLookupDatabaseModel> dbClient)
            where TLookupDatabaseModel : class
            where TAggregateDatabaseModel : class

        {

            var aggregatesCache =
                new AggregatesCacheManager<TAggregateDatabaseModel,
                    TLookupDatabaseModel>(
                    dbClient, new Cache<TAggregateDatabaseModel>());

            var nonDeletedItemsCache =
                CreateCategoryIndexCache(nonDeletedCategoryIndexKey, dbClient);

            var deletedItemsCache =
                CreateCategoryIndexCache(deletedCategoryIndexKey, dbClient);

            return new UnitOfWorkImp<TAggregateDatabaseModel, TLookupDatabaseModel>(
                dbClient,
                aggregatesCache,
                deletedItemsCache,
                nonDeletedItemsCache
            );
        }

        private ICategoryIndexCacheManager<TLookupDatabaseModel> CreateCategoryIndexCache
            <TAggregateDatabaseModel, TLookupDatabaseModel>(
                string categoryIndexKey,
                ITransactionalDatabaseClient<TAggregateDatabaseModel, TLookupDatabaseModel> dbClient)
            where TLookupDatabaseModel : class
            where TAggregateDatabaseModel : class
        {
          return new CategoryIndexCacheManager<TAggregateDatabaseModel, TLookupDatabaseModel>(
                categoryIndexKey,
                dbClient,
                new Cache<CategoryIndex<TLookupDatabaseModel>>());
        }
    }
}