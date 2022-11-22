using Support.UnitOfWork.Api;
using Testing.Common.Types;

namespace Testing.Common.Doubles
{
    internal class TransactionalDatabaseClient
        : ITransactionalDatabaseClient<AggregateDatabaseModel,
            LookupDatabaseModel>
    {
        private readonly InMemoryDataSource _ds;

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
                throw new NotImplementedException();
                //return Task.Run<IETagDto<AggregateDatabaseModel>?>(() =>
                //{

                //});
            }
        }

        /// <inheritdoc />
        public Task UpsertAggregateAsync(string key, string eTag,
            AggregateDatabaseModel aggregate,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<IETagDto<CategoryIndex<LookupDatabaseModel>>?>
            GetCategoryIndex(string categoryKey,
                CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task UpsertCategoryIndex(string key, string eTag,
            CategoryIndex<LookupDatabaseModel> categoryIndex,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task CommitTransactionAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                lock (_lockObject)
                {
                    _ds.Upsert(_aggregates, _categoryIndexes);
                }
            });
        }

        private static readonly object _lockObject = new();

        private readonly Dictionary<string, AggregateETag> _aggregates = new();

        private readonly Dictionary<string, CategoryIndexETag>
            _categoryIndexes = new();
    }

    internal class InMemoryDataSource
    {
        public AggregateETag? GetAggregate(string key)
        {
            if (!_aggregates.ContainsKey(key))
            {
                return null;
            }

            return _aggregates[key].Clone();
        }

        public CategoryIndexETag? GetCategoryIndex(string key)
        {
            if (!_categoryIndexes.ContainsKey(key))
            {
                return null;
            }

            return _categoryIndexes[key].Clone();
        }

        public void Upsert(Dictionary<string, AggregateETag> aggregates,
            Dictionary<string, CategoryIndexETag> categoryIndexes)
        {
            AssertETagsMatch(aggregates, categoryIndexes);

            SaveAggregates(aggregates);

            SaveCategoryIndexes(categoryIndexes);
        }

        private void SaveCategoryIndexes(
            Dictionary<string, CategoryIndexETag> categoryIndexes)
        {
            var updatedValues = categoryIndexes.ToDictionary(i => i.Key,
                i => i.Value.CloneWithNewETag());

            foreach (var index in updatedValues)
            {
                if (_categoryIndexes.ContainsKey(index.Key))
                {
                    _categoryIndexes[index.Key] = index.Value;
                }
                else
                {
                    _categoryIndexes.Add(index.Key, index.Value);
                }
            }
        }

        private void SaveAggregates(
            Dictionary<string, AggregateETag> aggregates)
        {
            var updatedAggregates = aggregates.ToDictionary(i => i.Key,
                i => i.Value.CloneWithNewETag());

            foreach (var aggregate in updatedAggregates)
            {
                if (_aggregates.ContainsKey(aggregate.Key))
                {
                    _aggregates[aggregate.Key] = aggregate.Value;
                }
                else
                {
                    _aggregates.Add(aggregate.Key, aggregate.Value);
                }
            }
        }


        private void AssertETagsMatch(
            Dictionary<string, AggregateETag> aggregates,
            Dictionary<string, CategoryIndexETag> categoryIndexes)
        {
            foreach (var aggregate in aggregates)
            {
                if (_aggregates.ContainsKey(aggregate.Key))
                {
                    if (_aggregates[aggregate.Key].Etag != aggregate.Value.Etag)
                    {
                        throw new DatabaseException("Aggregate Etag mistmatch");
                    }
                }
            }

            foreach (var index in categoryIndexes)
            {
                if (_categoryIndexes.ContainsKey(index.Key))
                {
                    if (_categoryIndexes[index.Key].Etag != index.Value.Etag)
                    {
                        throw new DatabaseException(
                            "CategoryIndex Etag Mismatch");
                    }
                }
            }
        }

        private readonly Dictionary<string, AggregateETag> _aggregates = new();

        private readonly Dictionary<string, CategoryIndexETag>
            _categoryIndexes = new();
    }

    internal class DatabaseException : Exception
    {
        public DatabaseException(string error) : base(error)
        {
        }
    }

    internal class AggregateETag : IETagDto<AggregateDatabaseModel>
    {
        public AggregateETag(string etag, AggregateDatabaseModel payload)
        {
            Etag = etag;
            Payload = payload;
        }

        /// <inheritdoc />
        public string Etag { get; }

        /// <inheritdoc />
        public AggregateDatabaseModel Payload { get; }

        public AggregateETag Clone()
        {
            return new(Etag, Payload);
        }

        public AggregateETag CloneWithNewETag()
        {
            return new(RandomString(), Payload);
        }
    }

    internal class
        CategoryIndexETag : IETagDto<CategoryIndex<LookupDatabaseModel>>
    {
        public CategoryIndexETag(string etag,
            CategoryIndex<LookupDatabaseModel> payload)
        {
            Etag = etag;
            Payload = payload;
        }

        /// <inheritdoc />
        public string Etag { get; }

        /// <inheritdoc />
        public CategoryIndex<LookupDatabaseModel> Payload { get; }

        public CategoryIndexETag Clone()
        {
            return new(Etag, Payload);
        }

        public CategoryIndexETag CloneWithNewETag()
        {
            return new(RandomString(), Payload);
        }
    }
}