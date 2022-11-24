using Common.InternalContracts;
using Jcg.Repositories.Api;
using Support.UnitOfWork.Cache;
using Support.UnitOfWork.Cache.Imp;

namespace Support.UnitOfWork
{
    internal static class UnitOfWorkFactory
    {
        public static IUnitOfWork<TAggregateDatabaseModel, TLookupDatabaseModel>
            Create<TAggregateDatabaseModel, TLookupDatabaseModel>(
                string nonDeletedCategoryIndexKey,
                string deletedCategoryIndexKey,
                ITransactionalDatabaseClient<TAggregateDatabaseModel,
                    TLookupDatabaseModel> dbClient)
            where TLookupDatabaseModel : ILookupDataModel
            where TAggregateDatabaseModel : class, IAggregateDataModel

        {
            var aggregatesCache =
                new AggregatesCacheManager<TAggregateDatabaseModel,
                    TLookupDatabaseModel>(
                    dbClient, new Cache<TAggregateDatabaseModel>());

            var nonDeletedItemsCache =
                CreateCategoryIndexCache(nonDeletedCategoryIndexKey, dbClient);

            var deletedItemsCache =
                CreateCategoryIndexCache(deletedCategoryIndexKey, dbClient);

            return new
                UnitOfWork<TAggregateDatabaseModel, TLookupDatabaseModel>(
                    dbClient,
                    aggregatesCache,
                    deletedItemsCache,
                    nonDeletedItemsCache
                );
        }

        private static ICategoryIndexCacheManager<TLookupDatabaseModel>
            CreateCategoryIndexCache
            <TAggregateDatabaseModel, TLookupDatabaseModel>(
                string categoryIndexKey,
                ITransactionalDatabaseClient<TAggregateDatabaseModel,
                    TLookupDatabaseModel> dbClient)
            where TLookupDatabaseModel : ILookupDataModel
            where TAggregateDatabaseModel : class, IAggregateDataModel
        {
            return new CategoryIndexCacheManager<TAggregateDatabaseModel,
                TLookupDatabaseModel>(
                categoryIndexKey,
                dbClient,
                new Cache<CategoryIndex<TLookupDatabaseModel>>());
        }
    }
}