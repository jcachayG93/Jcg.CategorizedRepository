using Common.Api;
using Common.InternalContracts;
using Support.DataModelRepository.Strategies;
using Support.UnitOfWork.Api;

namespace Support.DataModelRepository
{
    internal class DataModelRepository<TAggregateDatabaseModel,
            TLookupDatabaseModel>
        : IDataModelRepository<TAggregateDatabaseModel, TLookupDatabaseModel>
        where TAggregateDatabaseModel : class, IAggregateDataModel
        where TLookupDatabaseModel : ILookupDataModel
    {
        public DataModelRepository(
            IInitializeCategoryIndexStrategy initializeCategoryStrategy,
            IQueryStrategy<TAggregateDatabaseModel, TLookupDatabaseModel>
                queryStrategy,
            IUpsertAggregateStrategy<TAggregateDatabaseModel,
                TLookupDatabaseModel> upsertAggregateStrategy,
            IDeleteAndRestoreStrategy<TAggregateDatabaseModel,
                TLookupDatabaseModel> deleteAndRestoreStrategy,
            ICommitStrategy commitStrategy)
        {
            _initializeCategoryStrategy = initializeCategoryStrategy;
            _queryStrategy = queryStrategy;
            _upsertAggregateStrategy = upsertAggregateStrategy;
            _deleteAndRestoreStrategy = deleteAndRestoreStrategy;
            _commitStrategy = commitStrategy;
        }

        /// <inheritdoc />
        public Task InitializeCategoryIndexes(
            CancellationToken cancellationToken)
        {
            return _initializeCategoryStrategy.InitializeCategoryIndexes(
                cancellationToken);
        }

        /// <inheritdoc />
        public Task<TAggregateDatabaseModel?> GetAggregateAsync(
            Guid key,
            CancellationToken cancellationToken)
        {
            return _queryStrategy.GetAggregateAsync(key, cancellationToken);
        }

        /// <inheritdoc />
        public Task<CategoryIndex<TLookupDatabaseModel>>
            LookupNonDeletedAsync(
                CancellationToken cancellationToken)
        {
            return _queryStrategy.LookupNonDeletedAsync(cancellationToken);
        }

        /// <inheritdoc />
        public Task<CategoryIndex<TLookupDatabaseModel>>
            LookupDeletedAsync(
                CancellationToken cancellationToken)
        {
            return _queryStrategy.LookupDeletedAsync(cancellationToken);
        }

        /// <inheritdoc />
        public Task UpsertAsync(TAggregateDatabaseModel aggregate,
            CancellationToken cancellationToken)
        {
            return _upsertAggregateStrategy.UpsertAsync(aggregate,
                cancellationToken);
        }

        /// <inheritdoc />
        public Task DeleteAsync(Guid key, CancellationToken cancellationToken)
        {
            return _deleteAndRestoreStrategy.DeleteAsync(key,
                cancellationToken);
        }

        /// <inheritdoc />
        public Task RestoreAsync(Guid key, CancellationToken cancellationToken)
        {
            return _deleteAndRestoreStrategy.RestoreAsync(key,
                cancellationToken);
        }

        /// <inheritdoc />
        public Task CommitChangesAsync(CancellationToken cancellationToken)
        {
            return _commitStrategy.CommitChangesAsync(cancellationToken);
        }

        private readonly ICommitStrategy _commitStrategy;

        private readonly
            IDeleteAndRestoreStrategy<TAggregateDatabaseModel,
                TLookupDatabaseModel> _deleteAndRestoreStrategy;

        private readonly IInitializeCategoryIndexStrategy
            _initializeCategoryStrategy;

        private readonly
            IQueryStrategy<TAggregateDatabaseModel, TLookupDatabaseModel>
            _queryStrategy;

        private readonly
            IUpsertAggregateStrategy<TAggregateDatabaseModel,
                TLookupDatabaseModel> _upsertAggregateStrategy;
    }
}