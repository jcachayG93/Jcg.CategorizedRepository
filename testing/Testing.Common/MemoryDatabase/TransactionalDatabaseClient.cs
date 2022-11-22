using Support.UnitOfWork.Api;
using Testing.Common.Types;

namespace Testing.Common.Doubles
{
    public class TransactionalDatabaseClient
        : ITransactionalDatabaseClient<AggregateDatabaseModel,
            LookupDatabaseModel>
    {
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public CategoryIndexETag? GetCategoryIndex(string key)
        {
            throw new NotImplementedException();
        }

        public void Upsert(Dictionary<string, AggregateETag> aggregates,
            Dictionary<string, CategoryIndexETag> categoryIndexes)
        {
            throw new NotImplementedException();
        }

        private readonly Dictionary<string, AggregateETag> _aggregates = new();

        private readonly Dictionary<string, CategoryIndexETag>
            _categoryIndexes = new();
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
    }
}