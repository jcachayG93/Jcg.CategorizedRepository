using Support.UnitOfWork.Api;
using Support.UnitOfWork.Api.Exceptions;

namespace Support.UnitOfWork.Cache.Imp
{
    internal class CategoryIndexCacheManager<TAggregateDatabaseModel,
            TLookupDatabaseModel>
        : ICategoryIndexCacheManager<TLookupDatabaseModel>
        where TLookupDatabaseModel : class
        where TAggregateDatabaseModel : class
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="categoryKey">
        ///     The category key determines whether this is the Deleted or
        ///     Non Deleted items category index
        /// </param>
        public CategoryIndexCacheManager(
            string categoryKey,
            ITransactionalDatabaseClient<TAggregateDatabaseModel,
                TLookupDatabaseModel> databaseClient,
            Cache<CategoryIndex<TLookupDatabaseModel>> categoryIndexCache)
        {
            _categoryKey = categoryKey;
            _databaseClient = databaseClient;
            _categoryIndexCache = categoryIndexCache;
        }

        /// <inheritdoc />
        public UpsertedItem<CategoryIndex<TLookupDatabaseModel>>?
            UpsertedItem =>
            _categoryIndexCache.UpsertedItems.FirstOrDefault();

        /// <inheritdoc />
        public async Task<CategoryIndex<TLookupDatabaseModel>> GetAsync()
        {
            await ReadAndAddToCacheIfNeededAssertCacheNotEmptyAsync(
                _categoryKey);


            return _categoryIndexCache.Get(_categoryKey)!;
        }

        /// <inheritdoc />
        public async Task UpsertAsync(
            CategoryIndex<TLookupDatabaseModel> categoryIndex)
        {
            await ReadAndAddToCacheIfNeededAssertCacheNotEmptyAsync(
                _categoryKey);


            _categoryIndexCache.Upsert(_categoryKey, categoryIndex);
        }

        private async Task ReadAndAddToCacheIfNeededAssertCacheNotEmptyAsync(
            string key)
        {
            if (!_categoryIndexCache.HasKey(key))
            {
                var data =
                    await _databaseClient.GetCategoryIndex(key,
                        CancellationToken.None);

                if (data is null)
                {
                    throw new CategoryIndexIsUninitializedException();
                }

                _categoryIndexCache.Add(key, data);
            }
        }


        private readonly Cache<CategoryIndex<TLookupDatabaseModel>>
            _categoryIndexCache;

        private readonly string _categoryKey;

        private readonly
            ITransactionalDatabaseClient<TAggregateDatabaseModel,
                TLookupDatabaseModel> _databaseClient;
    }
}