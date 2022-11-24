using IntegrationTests.Common.Database;
using IntegrationTests.Common.Types;
using Jcg.DataAccessRepositories;

namespace IntegrationTests.Common.Parts
{
    public class
        TransactionalDatabaseClient : ITransactionalDatabaseClient<
            CustomerDataModel,
            CustomerLookupDataModel>
    {
        public TransactionalDatabaseClient(IInMemoryDatabase database)
        {
            _database = database;
        }

        /// <inheritdoc />
        public Task<IETagDto<CustomerDataModel>> GetAggregateAsync(string key,
            CancellationToken cancellationToken)
        {
            lock (LockObject)
            {
                var data = _database.Get(key);

                if (data is null)
                {
                    return Task.FromResult<IETagDto<CustomerDataModel>>(null);
                }

                var payload = (CustomerDataModel)data.Payload;

                payload = Clone(payload);

                var result =
                    new ETagDtoImp<CustomerDataModel>(data.ETag, payload);


                return Task.FromResult<IETagDto<CustomerDataModel>>(result)!;
            }
        }

        /// <inheritdoc />
        public Task UpsertAggregateAsync(string eTag,
            CustomerDataModel aggregate,
            CancellationToken cancellationToken)
        {
            lock (LockObject)
            {
                var op = new UpsertOperation(aggregate.Key, eTag,
                    Clone(aggregate));

                _operations.Add(op);

                return Task.CompletedTask;
            }
        }

        /// <inheritdoc />
        public Task<IETagDto<CategoryIndex<CustomerLookupDataModel>>>
            GetCategoryIndex(
                string categoryKey, CancellationToken cancellationToken)
        {
            lock (LockObject)
            {
                var data = _database.Get(categoryKey);

                if (data is null)
                {
                    return Task
                        .FromResult<
                            IETagDto<CategoryIndex<CustomerLookupDataModel>>>(
                            null);
                }

                var payload =
                    (CategoryIndex<CustomerLookupDataModel>)data.Payload;

                payload = Clone(payload);

                var result =
                    new ETagDtoImp<CategoryIndex<CustomerLookupDataModel>>(
                        data.ETag,
                        payload);


                return Task
                    .FromResult<
                        IETagDto<CategoryIndex<CustomerLookupDataModel>>>(
                        result)!;
            }
        }

        /// <inheritdoc />
        public Task UpsertCategoryIndex(string categoryKey, string eTag,
            CategoryIndex<CustomerLookupDataModel> categoryIndex,
            CancellationToken cancellationToken)
        {
            lock (LockObject)
            {
                var op = new UpsertOperation(categoryKey, eTag,
                    Clone(categoryIndex));

                _operations.Add(op);

                return Task.CompletedTask;
            }
        }

        /// <inheritdoc />
        public Task CommitTransactionAsync(CancellationToken cancellationToken)
        {
            lock (LockObject)
            {
                _database.UpsertAndCommit(_operations.ToList());

                _operations = new();

                return Task.CompletedTask;
            }
        }

        private static CategoryIndex<CustomerLookupDataModel> Clone(
            CategoryIndex<CustomerLookupDataModel> input)
        {
            var lookups = input.Lookups.Select(l =>
                new CustomerLookupDataModel
                {
                    CustomerName = l.CustomerName,
                    NumberOfOrders = l.NumberOfOrders,
                    Key = l.Key,
                    IsDeleted = l.IsDeleted,
                    DeletedTimeStamp = l.DeletedTimeStamp
                }).ToList();

            return new CategoryIndex<CustomerLookupDataModel>()
            {
                Lookups = lookups
            };
        }

        private static CustomerDataModel Clone(CustomerDataModel input)
        {
            var orders = input.Orders.Select(o =>
                new CustomerDataModel.OrderDataModel()
                {
                    Id = o.Id
                }).ToList();

            return new CustomerDataModel
            {
                Name = input.Name,
                Orders = orders,
                Key = input.Key
            };
        }

        private static readonly object LockObject = new();
        private readonly IInMemoryDatabase _database;

        private List<UpsertOperation> _operations = new();
    }
}