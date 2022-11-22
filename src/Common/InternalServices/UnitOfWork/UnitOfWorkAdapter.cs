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

        public Task<CategoryIndex<TLookupDatabaseModel>> GetNonDeletedItemsCategoryIndex(
            CancellationToken cancellationToken)
        {
            return _adaptee.GetNonDeletedItemsCategoryIndex(cancellationToken);
        }

        public Task<CategoryIndex<TLookupDatabaseModel>> GetDeletedItemsCategoryIndex(CancellationToken cancellationToken)
        {
            return _adaptee.GetDeletedItemsCategoryIndex(cancellationToken);
        }

        public Task UpsertDeletedItemsCategoryIndex(CategoryIndex<TLookupDatabaseModel> deletedItemsCategoryIndex, CancellationToken cancellationToken)
        {
            return _adaptee.UpsertDeletedItemsCategoryIndex(deletedItemsCategoryIndex, cancellationToken);
        }

        public Task UpsertNonDeletedItemsCategoryIndex(CategoryIndex<TLookupDatabaseModel> nonDeletedItemsCategoryIndex,
            CancellationToken cancellationToken)
        {
            return _adaptee.UpsertNonDeletedItemsCategoryIndex(nonDeletedItemsCategoryIndex, cancellationToken);
        }

        public Task<TAggregateDatabaseModel?> GetAggregateAsync(string key, CancellationToken cancellationToken)
        {
            return _adaptee.GetAggregateAsync(key, cancellationToken);
        }

        public Task UpsertAggregateAsync(string key, TAggregateDatabaseModel aggregate, CancellationToken cancellationToken)
        {
            return _adaptee.UpsertAggregateAsync(key, aggregate, cancellationToken);
        }

        public Task CommitChangesAsync(CancellationToken cancellationToken)
        {
            return _adaptee.CommitChangesAsync(cancellationToken);
        }
    }
}
