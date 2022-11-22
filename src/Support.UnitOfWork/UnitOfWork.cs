using Common.Api;
using Support.UnitOfWork.Api;
using Support.UnitOfWork.Api.Exceptions;
using Support.UnitOfWork.Cache;

namespace Support.UnitOfWork
{
    internal class
        UnitOfWork<TAggregateDatabaseModel, TLookupDatabaseModel> :
            IUnitOfWork<TAggregateDatabaseModel, TLookupDatabaseModel>
        where TAggregateDatabaseModel : class
        where TLookupDatabaseModel : class, IRepositoryKey
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

        /// <summary>
        ///     Gets the non deleted items category index
        /// </summary>
        /// <param name="categoryKey">The category key</param>
        /// <returns>The category index, null if it not initialized</returns>
        /// <exception cref="CategoryIndexIsUninitializedException">When the CategoryIndex is not found</exception>
        public Task<CategoryIndex<TLookupDatabaseModel>>
            GetNonDeletedItemsCategoryIndex(
                CancellationToken cancellationToken)
        {
            return _nonDeletedItemsCategoryIndexCache.GetAsync();
        }

        /// <summary>
        ///     Gets the deleted items category index
        /// </summary>
        /// <param name="categoryKey">The category key</param>
        /// <returns>The category index, null if it not initialized</returns>
        /// <exception cref="CategoryIndexIsUninitializedException">When the CategoryIndex is not found</exception>
        public Task<CategoryIndex<TLookupDatabaseModel>>
            GetDeletedItemsCategoryIndex(
                CancellationToken cancellationToken)
        {
            return _deletedItemsCategoryIndexCache.GetAsync();
        }

        /// <summary>
        ///     Upsert the deleted items category index
        /// </summary>
        public Task UpsertDeletedItemsCategoryIndex(
            CategoryIndex<TLookupDatabaseModel> deletedItemsCategoryIndex,
            CancellationToken cancellationToken)
        {
            AssertCommitWasNotCalled();
            return _deletedItemsCategoryIndexCache.UpsertAsync(
                deletedItemsCategoryIndex);
        }

        /// <summary>
        ///     Upsert the non-deleted items category index
        /// </summary>
        public Task UpsertNonDeletedItemsCategoryIndex(
            CategoryIndex<TLookupDatabaseModel> nonDeletedItemsCategoryIndex,
            CancellationToken cancellationToken)
        {
            AssertCommitWasNotCalled();
            return _nonDeletedItemsCategoryIndexCache.UpsertAsync(
                nonDeletedItemsCategoryIndex);
        }


        /// <summary>
        ///     Gets the aggregate for the matching key.
        /// </summary>
        /// <param name="key">The aggregate key key</param>
        /// <returns>The aggregate, null if not found</returns>
        public Task<TAggregateDatabaseModel?> GetAggregateAsync(string key,
            CancellationToken cancellationToken)
        {
            return _aggregatesCache.GetAsync(key);
        }

        /// <summary>
        ///     Upserts the aggregate
        /// </summary>
        /// <param name="key"></param>
        /// <param name="aggregate"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task UpsertAggregateAsync(string key,
            TAggregateDatabaseModel aggregate,
            CancellationToken cancellationToken)
        {
            AssertCommitWasNotCalled();
            return _aggregatesCache.UpsertAsync(key, aggregate);
        }

        /// <summary>
        ///     Commits all the changes. This operation can be called only once for the lifetime of this UnitOfWork
        /// </summary>
        /// <returns></returns>
        /// <exception cref="CommitMayBeCalledOnlyOnceException">
        ///     When called more than once. Once changes are committed, you must use a different instance for the
        ///     unit of work
        /// </exception>
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
                    _dbClient.UpsertAggregateAsync(i.Key, i.ETag, i.PayLoad!,
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