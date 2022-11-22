using Support.UnitOfWork;
using Support.UnitOfWork.Api;

namespace Common.InternalServices.UnitOfWork
{
    /// <summary>
    /// This is an adapter for factory defined in the Support.UnitOfWork project
    /// </summary>
    internal class UnitOfWorkFactory
    {
        public IUnitOfWork<TAggregateDatabaseModel, TLookupDatabaseModel>
            Create<TAggregateDatabaseModel, TLookupDatabaseModel>(
                string nonDeletedCategoryIndexKey,
                string deletedCategoryIndexKey,
                ITransactionalDatabaseClient<TAggregateDatabaseModel,
                    TLookupDatabaseModel> dbClient
            )
            where TAggregateDatabaseModel : class
            where TLookupDatabaseModel : class
        {
            var factoryAdaptee = new UnitOfWorkFactoryImp();

            var unitOfWorkAdaptee = factoryAdaptee.Create(nonDeletedCategoryIndexKey, deletedCategoryIndexKey, dbClient);

            return new UnitOfWorkAdapter<TAggregateDatabaseModel, TLookupDatabaseModel>(unitOfWorkAdaptee);
        }
    }
}