using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Support.UnitOfWork;
using Support.UnitOfWork.Api;

namespace Common.InternalServices.UnitOfWork
{
    internal class UnitOfWorkAdapter<TAggregateDatabaseModel,
        TLookupDatabaseModel>
    : IUnitOfWork<TAggregateDatabaseModel, TLookupDatabaseModel>
        where TAggregateDatabaseModel : class
        where TLookupDatabaseModel : class
    {
        private readonly IUnitOfWorkImp<TAggregateDatabaseModel, TLookupDatabaseModel> _adaptee;

        public UnitOfWorkAdapter(IUnitOfWorkImp<TAggregateDatabaseModel,TLookupDatabaseModel> adaptee)
        {
            _adaptee = adaptee;
        }
        public Task<CategoryIndex<TLookupDatabaseModel>> GetCategoryIndex(string categoryKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task UpsertCategoryIndex(string categoryKey, CategoryIndex<TLookupDatabaseModel> categoryIndex, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task CommitChangesAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<TAggregateDatabaseModel?> GetAggregateAsync(string key, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task UpsertAggregateAsync(string key, TAggregateDatabaseModel aggregate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
