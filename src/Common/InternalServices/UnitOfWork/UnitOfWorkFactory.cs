using Support.UnitOfWork.Api;

namespace Common.InternalServices.UnitOfWork
{
    internal class UnitOfWorkFactory
    {
        public IUnitOfWork<TAggregateDatabaseModel, TLookupDatabaseModel>
            Create<TAggregateDatabaseModel, TLookupDatabaseModel>(
                ITransactionalDatabaseClient<TAggregateDatabaseModel,
                    TLookupDatabaseModel> dbClient
            )
            where TAggregateDatabaseModel : class
            where TLookupDatabaseModel : class
        {
            throw new NotImplementedException();
        }
    }
}