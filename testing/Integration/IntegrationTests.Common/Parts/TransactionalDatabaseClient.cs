﻿using IntegrationTests.Common.Database;
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
                var data = _database.Get(key);

                if (data is null)
                {
                    return Task.FromResult<IETagDto<CustomerDataModel>?>(null);
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
            lock (_lockObject)
            {
                var op = new UpsertOperation(aggregate.Key, eTag,
                    Clone(aggregate));

                _operations.Add(op);

                return Task.CompletedTask;
            }
        }

        /// <inheritdoc />
        public Task<IETagDto<CategoryIndex<LookupDataModel>>?> GetCategoryIndex(
            string categoryKey, CancellationToken cancellationToken)
        {
            lock (_lockObject)
            {
                var data = _database.Get(categoryKey);

                if (data is null)
                {
                    return Task
                        .FromResult<IETagDto<CategoryIndex<LookupDataModel>>?>(
                            null);
                }

                var payload = (CategoryIndex<LookupDataModel>)data.Payload;

                payload = Clone(payload);

                var result =
                    new ETagDtoImp<CategoryIndex<LookupDataModel>>(data.ETag,
                        payload);


                return Task
                    .FromResult<IETagDto<CategoryIndex<LookupDataModel>>>(
                        result)!;
            }
        }

        /// <inheritdoc />
        public Task UpsertCategoryIndex(string categoryKey, string eTag,
            CategoryIndex<LookupDataModel> categoryIndex,
            CancellationToken cancellationToken)
        {
            lock (_lockObject)
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
            lock (_lockObject)
            {
                _database.UpsertAndCommit(_operations.ToList());

                _operations = new();

                return Task.CompletedTask;
            }
        }

        private CategoryIndex<LookupDataModel> Clone(
            CategoryIndex<LookupDataModel> input)
        {
            var lookups = input.Lookups.Select(l =>
                new LookupDataModel
                {
                    CustomerName = l.CustomerName,
                    NumberOfOrders = l.NumberOfOrders,
                    Key = l.Key,
                    IsDeleted = l.IsDeleted,
                    DeletedTimeStamp = l.DeletedTimeStamp
                }).ToList();

            return new CategoryIndex<LookupDataModel>()
            {
                Lookups = lookups
            };
        }

        private CustomerDataModel Clone(CustomerDataModel input)
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

        private static readonly object _lockObject = new object();
        private readonly IInMemoryDatabase _database;

        private List<UpsertOperation> _operations = new();
    }
}