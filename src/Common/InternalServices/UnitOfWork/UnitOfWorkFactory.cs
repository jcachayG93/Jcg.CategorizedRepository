using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Support.UnitOfWork.Api;

namespace Common.InternalServices.UnitOfWork
{
    internal class UnitOfWorkFactory
    {
        IUnitOfWork<TAggregateDatabaseModel, TLookupDatabaseModel> 
            Create<TAggregateDatabaseModel, TLookupDatabaseModel>(
                ITransactionalDatabaseClient<TAggregateDatabaseModel, TLookupDatabaseModel> dbClient
                )
            where TAggregateDatabaseModel : class
            where TLookupDatabaseModel : class
        {
            throw new NotImplementedException();
        }
    }
}
