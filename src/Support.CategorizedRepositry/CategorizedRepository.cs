using Common.Api;
using Common.Api.Api;
using Common.InternalContracts;
using Support.CategorizedRepository.Support;
using System.Linq;

namespace Support.CategorizedRepository
{
    internal class CategorizedRepository<TAggregate, TAggregateDatabaseModel, TLookup, TLookupDatabaseModel>
    : ICategorizedRepository<TAggregate, TLookup>
        where TAggregateDatabaseModel : class, IAggregateDataModel
        where TLookupDatabaseModel : IRepositoryLookup
    {
     
        public CategorizedRepository(
            IAggregateMapper<TAggregate, TAggregateDatabaseModel> aggregateMapper,
            IAggregateToLookupMapper<TAggregateDatabaseModel, TLookupDatabaseModel> aggregateToLookupMapper,
            ILookupMapperAdapter<TLookupDatabaseModel, TLookup> lookupMapper,
            IDataModelRepository<TAggregateDatabaseModel,TLookupDatabaseModel> dataModelRepository)
        {
            
        }
        public Task InitializeCategoryIndexAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<TAggregate?> GetAggregateAsync(RepositoryIdentity key, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task UpsertAsync(RepositoryIdentity key, 
            TAggregate aggregate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TLookup>> LookupNonDeletedAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TLookup>> LookupDeletedAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(RepositoryIdentity key, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RestoreAsync(RepositoryIdentity key, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task CommitChangesAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
