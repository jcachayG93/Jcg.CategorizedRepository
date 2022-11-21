using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Support.UnitOfWork.Api;

namespace Support.UnitOfWork.Cache.Imp
{
    internal class AggregatesCacheManager<TAggregateDatabaseModel, TLookupDatabaseModel>
    : IAggregatesCacheManager<TAggregateDatabaseModel>
        where TAggregateDatabaseModel : class
    where TLookupDatabaseModel : class
    {
        public AggregatesCacheManager(
            ITransactionalDatabaseClient<TAggregateDatabaseModel, TLookupDatabaseModel> dbClient)
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

        public IEnumerable<UpsertedItem<TAggregateDatabaseModel>> UpsertedItems { get; }
    }
}
