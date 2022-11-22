using Support.UnitOfWork.Api;
using Testing.Common.Types;

namespace Testing.Common.Doubles
{
    public class TransactionalDatabaseClientDouble
        : ITransactionalDatabaseClient<AggregateDatabaseModel,
            LookupDatabaseModel>
    {
        /// <inheritdoc />
        public Task<IETagDto<AggregateDatabaseModel>?> GetAggregateAsync(
            string key, CancellationToken cancellationToken)
        {
            return Task.Run<IETagDto<AggregateDatabaseModel>?>(() =>
            {
                lock (_lockObject)
                {
                    if (!_data.ContainsKey(key))
                    {
                        return null;
                    }

                    var item = _data[key];

                    return new AggregateETag(item.Etag,
                        (AggregateDatabaseModel)item.Payload);
                }
            });
        }

        /// <inheritdoc />
        public Task UpsertAggregateAsync(string key, string eTag,
            AggregateDatabaseModel aggregate,
            CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                lock (_lockObject)
                {
                    var dto = new AggregateETag(eTag, aggregate);

                    if (!_data.ContainsKey(key))
                    {
                        var item = new DatabaseItem(RandomString(), aggregate);

                        _data.Add(key, item);
                    }
                    else
                    {
                        var item = _data[key];

                        item = Update(item, aggregate);

                        _data[key] = item;
                    }
                }
            });
        }

        /// <inheritdoc />
        public Task<IETagDto<CategoryIndex<LookupDatabaseModel>>?>
            GetCategoryIndex(string categoryKey,
                CancellationToken cancellationToken)
        {
            return Task.Run<IETagDto<CategoryIndex<LookupDatabaseModel>>?>(() =>
            {
                lock (_lockObject)
                {
                    if (!_data.ContainsKey(categoryKey))
                    {
                        return null;
                    }

                    var item = _data[categoryKey];

                    return new CategoryIndexETag(item.Etag,
                        (CategoryIndex<LookupDatabaseModel>)item.Payload);
                }
            });
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

        private DatabaseItem Update(DatabaseItem item, object newPayload)
        {
            return item with
            {
                Etag = Guid.NewGuid().ToString(),
                Payload = newPayload
            };
        }

        private static readonly object _lockObject = new object();

        /// <summary>
        ///     The database, the key is the entity id
        /// </summary>
        private readonly Dictionary<string, DatabaseItem> _data = new();

        private record DatabaseItem(string Etag, object Payload);
    }
}