using Jcg.CategorizedRepository.Api;
using Jcg.CategorizedRepository.DataModelRepo;

namespace Jcg.CategorizedRepository.CategorizedRepo
{
    internal class CategorizedRepository<TAggregate, TAggregateDatabaseModel,
            TLookup>
        : ICategorizedRepository<TAggregate, TLookup>
        where TAggregateDatabaseModel : class, IAggregateDataModel
    {
        public CategorizedRepository(
            IAggregateMapper<TAggregate, TAggregateDatabaseModel>
                aggregateMapper,
            IDataModelRepository<TAggregateDatabaseModel, TLookup>
                dataModelRepository)
        {
            _aggregateMapper = aggregateMapper;
            _dataModelRepository = dataModelRepository;
        }

        public Task InitializeCategoryIndexAsync(
            CancellationToken cancellationToken)
        {
            return _dataModelRepository.InitializeCategoryIndexes(
                cancellationToken);
        }

        public async Task<TAggregate?> GetAggregateAsync(RepositoryIdentity key,
            CancellationToken cancellationToken)
        {
            var data =
                await _dataModelRepository.GetAggregateAsync(key.Value,
                    CancellationToken.None);

            return data is null
                ? default(TAggregate)
                : _aggregateMapper.ToAggregate(data);
        }

        public async Task UpsertAsync(RepositoryIdentity key,
            TAggregate aggregate, CancellationToken cancellationToken)
        {
            var dataModel = _aggregateMapper.ToDatabaseModel(aggregate);

            if (dataModel != null)
            {
                await _dataModelRepository.UpsertAsync(dataModel,
                    CancellationToken.None);
            }
        }


        /// <inheritdoc />
        public async Task<IEnumerable<LookupDto<TLookup>>>
            LookupNonDeletedAsync(
                CancellationToken cancellationToken)
        {
            var data =
                await _dataModelRepository.LookupNonDeletedAsync(
                    cancellationToken);

            return data.Lookups;
        }


        /// <inheritdoc />
        public async Task<IEnumerable<LookupDto<TLookup>>> LookupDeletedAsync(
            CancellationToken cancellationToken)
        {
            var data =
                await _dataModelRepository
                    .LookupDeletedAsync(cancellationToken);

            return data.Lookups;
        }

        public Task DeleteAsync(RepositoryIdentity key,
            CancellationToken cancellationToken)
        {
            return _dataModelRepository.DeleteAsync(key.Value,
                cancellationToken);
        }

        public Task RestoreAsync(RepositoryIdentity key,
            CancellationToken cancellationToken)
        {
            return _dataModelRepository.RestoreAsync(key.Value,
                cancellationToken);
        }

        public Task CommitChangesAsync(CancellationToken cancellationToken)
        {
            return _dataModelRepository.CommitChangesAsync(cancellationToken);
        }

        private readonly IAggregateMapper<TAggregate, TAggregateDatabaseModel>
            _aggregateMapper;

        private readonly IDataModelRepository<TAggregateDatabaseModel, TLookup>
            _dataModelRepository;
    }
}