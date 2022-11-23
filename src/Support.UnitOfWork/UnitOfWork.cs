using Common.Api;
using Common.Api.Exceptions;
using Support.UnitOfWork.Api;
using Support.UnitOfWork.Api.Exceptions;
using Support.UnitOfWork.Cache;

namespace Support.UnitOfWork
{
    internal class
        UnitOfWork<TAggregateDatabaseModel, TLookupDatabaseModel> :
            IUnitOfWork<TAggregateDatabaseModel, TLookupDatabaseModel>
        where TAggregateDatabaseModel : class, IAggregateDataModel
        where TLookupDatabaseModel : IRepositoryLookup
    {
        public UnitOfWork(
            ITransactionalDatabaseClient<TAggregateDatabaseModel,
                TLookupDatabaseModel> dbClient,
            IAggregatesCacheManager<TAggregateDatabaseModel> aggregatesCache,
            ICategoryIndexCacheManager<TLookupDatabaseModel>
                deletedItemsCategoryIndexCache,
            ICategoryIndexCacheManager<TLookupDatabaseModel>
                nonDeletedItemsCategoryIndexCache)
        {
            _dbClient = dbClient;
            _aggregatesCache = aggregatesCache;
            _deletedItemsCategoryIndexCache = deletedItemsCategoryIndexCache;
            _nonDeletedItemsCategoryIndexCache =
                nonDeletedItemsCategoryIndexCache;

            _commitWasCalled = false;
        }

        /// <inheritdoc />
        public Task<CategoryIndex<TLookupDatabaseModel>>
            GetNonDeletedItemsCategoryIndex(
                CancellationToken cancellationToken)
        {
            return _nonDeletedItemsCategoryIndexCache.GetAsync();
        }

        /// <inheritdoc />
        public Task<CategoryIndex<TLookupDatabaseModel>>
            GetDeletedItemsCategoryIndex(
                CancellationToken cancellationToken)
        {
            return _deletedItemsCategoryIndexCache.GetAsync();
        }

        /// <inheritdoc />
        public async Task<bool> CategoryIndexIsInitializedAsync(
            CancellationToken cancellationToken)
        {
            var deletedIndexExist =
                await _deletedItemsCategoryIndexCache.IndexExistsAsync();

            var nonDeletedIndexExist =
                await _nonDeletedItemsCategoryIndexCache.IndexExistsAsync();

            if (deletedIndexExist != nonDeletedIndexExist)
            {
                var errorMessage = deletedIndexExist
                    ? "Deleted items Category Index Exists but Non Deleted Category index does not."
                    : "Non Deleted items Category Index Exists but Deleted Category index does not.";

                throw new InternalRepositoryErrorException(errorMessage);
            }

            return deletedIndexExist;
        }

        /// <inheritdoc />
        public Task UpsertDeletedItemsCategoryIndex(
            CategoryIndex<TLookupDatabaseModel> deletedItemsCategoryIndex,
            CancellationToken cancellationToken)
        {
            AssertCommitWasNotCalled();
            return _deletedItemsCategoryIndexCache.UpsertAsync(
                deletedItemsCategoryIndex);
        }

        /// <inheritdoc />
        public Task UpsertNonDeletedItemsCategoryIndex(
            CategoryIndex<TLookupDatabaseModel> nonDeletedItemsCategoryIndex,
            CancellationToken cancellationToken)
        {
            AssertCommitWasNotCalled();
            return _nonDeletedItemsCategoryIndexCache.UpsertAsync(
                nonDeletedItemsCategoryIndex);
        }


        /// <inheritdoc />
        public Task<TAggregateDatabaseModel?> GetAggregateAsync(string key,
            CancellationToken cancellationToken)
        {
            return _aggregatesCache.GetAsync(key);
        }


        /// <inheritdoc />
        public Task UpsertAggregateAsync(TAggregateDatabaseModel aggregate,
            CancellationToken cancellationToken)
        {
            AssertCommitWasNotCalled();
            return _aggregatesCache.UpsertAsync(aggregate);
        }

        /// <inheritdoc />
        public async Task CommitChangesAsync(
            CancellationToken cancellationToken)
        {
            AssertCommitWasNotCalled();

            _commitWasCalled = true;

            var anyChanges = await UpsertAllUpsertedAggregates();

            if (await UpsertDeletedItemsCategoryIndex())
            {
                anyChanges = true;
            }

            if (await UpsertNonDeletedIndexCategoryIndex())
            {
                anyChanges = true;
            }


            if (anyChanges)
            {
                await _dbClient.CommitTransactionAsync(cancellationToken);
            }
        }

        private void AssertCommitWasNotCalled()
        {
            if (_commitWasCalled)
            {
                throw new UnitOfWorkWasAlreadyCommittedException();
            }
        }


        private async Task<bool> UpsertDeletedItemsCategoryIndex()
        {
            var item = _deletedItemsCategoryIndexCache.UpsertedItem;

            if (item is null)
            {
                return false;
            }

            await _dbClient.UpsertCategoryIndex(item.Key, item.ETag,
                item.PayLoad, CancellationToken.None);

            return true;
        }

        private async Task<bool> UpsertNonDeletedIndexCategoryIndex()
        {
            var item = _nonDeletedItemsCategoryIndexCache.UpsertedItem;

            if (item is null)
            {
                return false;
            }

            await _dbClient.UpsertCategoryIndex(item.Key, item.ETag,
                item.PayLoad, CancellationToken.None);

            return true;
        }

        private async Task<bool> UpsertAllUpsertedAggregates()
        {
            if (!_aggregatesCache.UpsertedItems.Any())
            {
                return false;
            }

            var taskList = _aggregatesCache
                .UpsertedItems.Select(i =>
                    _dbClient.UpsertAggregateAsync(i.ETag, i.PayLoad!,
                        CancellationToken.None))
                .ToList();

            await Task.WhenAll(taskList);

            return true;
        }

        private readonly IAggregatesCacheManager<TAggregateDatabaseModel>
            _aggregatesCache;

        private readonly
            ITransactionalDatabaseClient<TAggregateDatabaseModel,
                TLookupDatabaseModel> _dbClient;

        private readonly ICategoryIndexCacheManager<TLookupDatabaseModel>
            _deletedItemsCategoryIndexCache;

        private readonly ICategoryIndexCacheManager<TLookupDatabaseModel>
            _nonDeletedItemsCategoryIndexCache;

        private bool _commitWasCalled;
    }
}