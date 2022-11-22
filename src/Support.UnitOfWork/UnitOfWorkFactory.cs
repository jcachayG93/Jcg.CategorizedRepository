using Support.UnitOfWork.Api;
using Support.UnitOfWork.Cache.Imp;

namespace Support.UnitOfWork
{
    internal class UnitOfWorkFactory
    {
        public IUnitOfWorkImp<TAggregateDatabaseModel, TLookupDatabaseModel>
            Create<TAggregateDatabaseModel, TLookupDatabaseModel>(
                ITransactionalDatabaseClient<TAggregateDatabaseModel,
                    TLookupDatabaseModel> dbClient)
            where TLookupDatabaseModel : class
            where TAggregateDatabaseModel : class

        {
            here
            //var aggregatesCache =
            //    new AggregatesCacheManager<TAggregateDatabaseModel,
            //        TLookupDatabaseModel>(
            //        dbClient, new Cache<TAggregateDatabaseModel>());

            //var deletedItemsCache = new CategoryIndexCacheManager<
            //TAggregateDatabaseModel, TLookupDatabaseModel>(
            //    RandomString(),
            //    dbClient, new Cache<CategoryIndex<TLookupDatabaseModel>>())
            //throw new NotImplementedException();
            //return new UnitOfWorkImp<TAggregateDatabaseModel,
            //    TLookupDatabaseModel>(
            //    dbClient,
            //    new AggregatesCacheManager<TAggregateDatabaseModel,TLookupDatabaseModel>(dbClient, new Cache<TAggregateDatabaseModel>()),
            //    new Catego,
            //    nonDeletedItemsCategoryIndexCache);
        }
    }
}