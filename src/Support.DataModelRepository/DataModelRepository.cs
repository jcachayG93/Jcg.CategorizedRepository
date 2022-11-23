using Common.Api;
using Common.InternalContracts;
using Support.DataModelRepository.Strategies;
using Support.UnitOfWork;
using Support.UnitOfWork.Api;

namespace Support.DataModelRepository
{
    internal class DataModelRepository<TAggregateDatabaseModel,
            TLookupDatabaseModel>
        : IDataModelRepository<TAggregateDatabaseModel, TLookupDatabaseModel>
        where TAggregateDatabaseModel : class, IAggregateDataModel
        where TLookupDatabaseModel : IRepositoryLookup
    {
        public DataModelRepository(
            IUnitOfWork<TAggregateDatabaseModel, TLookupDatabaseModel>
                unitOfWork,
            IInitializeCategoryIndexStrategy initializeCategoryStrategy,
            IQueryStrategy<TAggregateDatabaseModel, TLookupDatabaseModel>
                queryStrategy,
            IUpsertAggregateStrategy<TAggregateDatabaseModel,
                TLookupDatabaseModel> upsertAggregateStrategy,
            IDeleteAndRestoreStrategy<TAggregateDatabaseModel,
                TLookupDatabaseModel> deleteAndRestoreStrategy)
        {
        }

        /// <inheritdoc />
        public Task InitializeCategoryIndexes(
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<TAggregateDatabaseModel?> GetAggregateAsync(Guid key,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<CategoryIndex<TLookupDatabaseModel>> LookupNonDeletedAsync(
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<CategoryIndex<TLookupDatabaseModel>> LookupDeletedAsync(
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task UpsertAsync(Guid key, TAggregateDatabaseModel aggregate,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task DeleteAsync(Guid key, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task RestoreAsync(Guid key, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task CommitChangesAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}