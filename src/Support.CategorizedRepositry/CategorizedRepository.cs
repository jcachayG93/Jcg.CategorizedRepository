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
        where TLookupDatabaseModel : ILookupDataModel
    {
        private readonly IAggregateMapper<TAggregate, TAggregateDatabaseModel> _aggregateMapper;
        private readonly ILookupMapperAdapter<TLookupDatabaseModel, TLookup> _lookupMapper;
        private readonly IDataModelRepository<TAggregateDatabaseModel, TLookupDatabaseModel> _dataModelRepository;

        public CategorizedRepository(
            IAggregateMapper<TAggregate, TAggregateDatabaseModel> aggregateMapper,
            ILookupMapperAdapter<TLookupDatabaseModel, TLookup> lookupMapper,
            IDataModelRepository<TAggregateDatabaseModel,TLookupDatabaseModel> dataModelRepository)
        {
            _aggregateMapper = aggregateMapper;
            _lookupMapper = lookupMapper;
            _dataModelRepository = dataModelRepository;
        }
        public Task InitializeCategoryIndexAsync(CancellationToken cancellationToken)
        {
            return _dataModelRepository.InitializeCategoryIndexes(cancellationToken);
        }

        public async Task<TAggregate?> GetAggregateAsync(RepositoryIdentity key, CancellationToken cancellationToken)
        {
            var data = await _dataModelRepository.GetAggregateAsync(key.Value, CancellationToken.None);

            return data is null ? default(TAggregate) : _aggregateMapper.ToAggregate(data);
        }

        public async Task UpsertAsync(RepositoryIdentity key, 
            TAggregate aggregate, CancellationToken cancellationToken)
        {
            var dataModel = _aggregateMapper.ToDatabaseModel(aggregate);

            if (dataModel != null)
            {
                await _dataModelRepository.UpsertAsync(dataModel, CancellationToken.None);
            }
        }

        public async Task<IEnumerable<TLookup>> LookupNonDeletedAsync(CancellationToken cancellationToken)
        {
            var data = await _dataModelRepository.LookupNonDeletedAsync(cancellationToken);

            return _lookupMapper.Map(data);
        }

        public async Task<IEnumerable<TLookup>> LookupDeletedAsync(CancellationToken cancellationToken)
        {
            var data = await _dataModelRepository.LookupDeletedAsync(cancellationToken);

            return _lookupMapper.Map(data);
        }

        public Task DeleteAsync(RepositoryIdentity key, CancellationToken cancellationToken)
        {
            return _dataModelRepository.DeleteAsync(key.Value, cancellationToken);
        }

        public Task RestoreAsync(RepositoryIdentity key, CancellationToken cancellationToken)
        {
            return _dataModelRepository.RestoreAsync(key.Value, cancellationToken);
        }

        public Task CommitChangesAsync(CancellationToken cancellationToken)
        {
            return _dataModelRepository.CommitChangesAsync(cancellationToken);
        }
    }
}
