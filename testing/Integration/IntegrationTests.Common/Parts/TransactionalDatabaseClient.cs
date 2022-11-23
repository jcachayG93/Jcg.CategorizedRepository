using IntegrationTests.Common.Database;
using IntegrationTests.Common.Types;
using Support.UnitOfWork.Api;

namespace IntegrationTests.Common.Parts
{
    public class
        TransactionalDatabaseClient : ITransactionalDatabaseClient<
            CustomerDataModel,
            LookupDataModel>
    {
        public TransactionalDatabaseClient(IInMemoryDatabase database)
        {
            _database = database;
        }

        /// <inheritdoc />
        public Task<IETagDto<CustomerDataModel>?> GetAggregateAsync(string key,
            CancellationToken cancellationToken)
        {
            lock (_lockObject)
            {
                return Task.FromResult(
                    _database.GetAggregate<CustomerDataModel>(key));
            }
        }

        /// <inheritdoc />
        public Task UpsertAggregateAsync(string eTag,
            CustomerDataModel aggregate,
            CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                lock (_lockObject)
                {
                    var op =
                        new UpsertOperation(aggregate.Key, eTag, aggregate);

                    if (_operations.ContainsKey(aggregate.Key))
                    {
                        _operations[aggregate.Key] = op;
                    }
                    else
                    {
                        _operations.Add(aggregate.Key, op);
                    }
                }
            });
        }

        /// <inheritdoc />
        public Task<IETagDto<CategoryIndex<LookupDataModel>>?> GetCategoryIndex(
            string categoryKey, CancellationToken cancellationToken)
        {
            lock (_lockObject)
            {
                return Task.FromResult(
                    _database.GetAggregate<CategoryIndex<LookupDataModel>>(
                        categoryKey));
            }
        }

        /// <inheritdoc />
        public Task UpsertCategoryIndex(string categoryKey, string eTag,
            CategoryIndex<LookupDataModel> categoryIndex,
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

        private readonly IInMemoryDatabase _database;

        private readonly Dictionary<string, UpsertOperation>
            _operations = new();
    }
}