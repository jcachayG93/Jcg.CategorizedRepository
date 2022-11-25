using Jcg.CategorizedRepository.Api;
using Jcg.CategorizedRepository.UoW.Cache;
using Jcg.CategorizedRepository.UoW.Cache.Imp;

namespace Jcg.CategorizedRepository.UoW
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