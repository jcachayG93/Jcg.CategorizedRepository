using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegrationTests.Common.Types;
using Support.UnitOfWork.Api;

namespace IntegrationTests.Common.Database
{
    public interface IInMemoryDatabase
    {
        IETagDto<CustomerDataModel>? GetAggregate(string key);

        IETagDto<CategoryIndex<LookupDataModel>>? GetCategoryIndex(string key);

        /// <summary>
        /// Applies all the operations in a single batch
        /// </summary>
        void UpsertDataAndCommit(
            IEnumerable<UpsertAggregateOperation> aggregateOps,
            IETagDto<UpsertIndexOperation> indexOps);

        
    }

    public class InMemoryDatabase : IInMemoryDatabase
    {
        private static readonly object _lockObject = new();

        public IETagDto<CustomerDataModel>? GetAggregate(string key)
        {
            lock (_lockObject)
            {
                if (_aggregates.TryGetValue(key, out var value))
                {
                    return new ETagDto<CustomerDataModel>(value.Etag, value.Payload);
                }

                return null;
            }
        }

        public IETagDto<CategoryIndex<LookupDataModel>>? GetCategoryIndex(string key)
        {
            lock (_lockObject)
            {
                if (_indexes.TryGetValue(key, out var value))
                {
                    return new ETagDto<CategoryIndex<LookupDataModel>>(value.Etag, value.Payload);
                }

                return null;
            }
        }

        public void UpsertDataAndCommit(
            IEnumerable<UpsertAggregateOperation> aggregateOps, 
            IETagDto<UpsertIndexOperation> indexOps)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// If the ETag is blank, asserts the key is not assigned in the database (insert), otherwise, asserts the ETag matches
        /// the database version
        /// </summary>
        /// <param name="aggregateOps"></param>
        /// <param name="indexOps"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void AssertTagsMatch(IEnumerable<UpsertAggregateOperation> aggregateOps,
            IETagDto<UpsertIndexOperation> indexOps)
        {
            throw new NotImplementedException();
        }

        private void HandleInserts(IEnumerable<UpsertAggregateOperation> aggregateOps,
            IEnumerable<UpsertIndexOperation> indexOps)
        {
            throw new NotImplementedException();
        }


        private class ETagDto<T> : IETagDto<T>

        {
            public ETagDto(string etag, T payload)
            {
                Etag = etag;
                Payload = payload;
            }

            public string Etag { get; }
            public T Payload { get; }
        }

        private Dictionary<string, AggregateDto> _aggregates = new();

        private Dictionary<string, IndexDto> _indexes = new();

        private record AggregateDto(string Etag, CustomerDataModel Payload);

        private record IndexDto(string Etag, string Key, CategoryIndex<LookupDataModel> Payload);
    }

    public class InMemoryDatabaseException : Exception
    {
        public InMemoryDatabaseException(string error)
        : base(error)
        {
            
        }
    }

    public record UpsertAggregateOperation(string ETag, CustomerDataModel Payload);

    public record UpsertIndexOperation(string ETag, string Key,
        CategoryIndex<LookupDataModel> Payload);


}
