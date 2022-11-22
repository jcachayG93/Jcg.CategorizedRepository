using Support.UnitOfWork.Api;
using Testing.Common.Types;

namespace Testing.Common.Doubles
{
    public class TransactionalDatabaseClient
        : ITransactionalDatabaseClient<AggregateDatabaseModel,
            LookupDatabaseModel>
    {
        public TransactionalDatabaseClient(InMemoryDataSource ds)
        {
            _ds = ds;
        }

        /// <inheritdoc />
        public Task<IETagDto<AggregateDatabaseModel>?> GetAggregateAsync(
            string key, CancellationToken cancellationToken)
        {
            lock (_lockObject)
            {
                var result = _ds.GetAggregate(key);

                return Task.FromResult<IETagDto<AggregateDatabaseModel>?>(
                    result);
            }
        }

        /// <inheritdoc />
        public Task UpsertAggregateAsync(string key, string eTag,
            AggregateDatabaseModel aggregate,
            CancellationToken cancellationToken)
        {
            lock (_lockObject)
            {
                var item = new AggregateETag(eTag, aggregate);

                if (_aggregateChanges.ContainsKey(key))
                {
                    _aggregateChanges[key] = item;
                }
                else
                {
                    _aggregateChanges.Add(key, item);
                }

                return Task.CompletedTask;
            }
        }

        /// <inheritdoc />
        public Task<IETagDto<CategoryIndex<LookupDatabaseModel>>?>
            GetCategoryIndex(string categoryKey,
                CancellationToken cancellationToken)
        {
            lock (_lockObject)
            {
                var result = _ds.GetCategoryIndex(categoryKey);

                return Task
                    .FromResult<IETagDto<CategoryIndex<LookupDatabaseModel>>?>(
                        result);
            }
        }

        /// <inheritdoc />
        public Task UpsertCategoryIndex(string categoryKey, string eTag,
            CategoryIndex<LookupDatabaseModel> categoryIndex,
            CancellationToken cancellationToken)
        {
            lock (_lockObject)
            {
                var item = new CategoryIndexETag(eTag, categoryIndex);

                if (_categoryIndexChanges.ContainsKey(categoryKey))
                {
                    _categoryIndexChanges[categoryKey] = item;
                }
                else
                {
                    _categoryIndexChanges.Add(categoryKey, item);
                }

                return Task.CompletedTask;
            }
        }

        /// <inheritdoc />
        public Task CommitTransactionAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                lock (_lockObject)
                {
                    _ds.Upsert(_aggregateChanges, _categoryIndexChanges);
                }
            });
        }

        private static readonly object _lockObject = new();

        private readonly Dictionary<string, AggregateETag> _aggregateChanges =
            new();

        private readonly Dictionary<string, CategoryIndexETag>
            _categoryIndexChanges = new();

        private readonly InMemoryDataSource _ds;
    }
}