using FluentAssertions;
using Jcg.DataAccessRepositories;
using Testing.Common.MemoryDatabase;
using Testing.Common.Support.Assertions;
using Testing.Common.Support.Extensions;
using Testing.Common.Types;

namespace Support.UnitOfWork.TestCommon.MockDatabase
{
    public class TransactionalDatabaseClientTests
    {
        /*
         * Making sure the test DatabaseClient works
         * because integration test depend on it.
         * The SUT is not part of the library but the testing projects.
         */
        public TransactionalDatabaseClientTests()
        {
            var ds = new InMemoryDataSource();

            Sut = new(ds);
        }

        private TransactionalDatabaseClient Sut { get; }

        private AggregateDatabaseModel RandomAggregate()
        {
            return new()
            {
                SomeValue = RandomString(),
                Key = RandomString()
            };
        }

        private void CreateUpsertAggregateData(
            out AggregateDatabaseModel aggregate,
            out string Key,
            out string Etag)
        {
            Etag = RandomString();

            aggregate = RandomAggregate();
            Key = aggregate.Key;
        }

        private void CreateUpsertCategoryIndexData(
            out CategoryIndex<LookupDatabaseModel> categoryIndex,
            out string Key,
            out string Etag)
        {
            Key = RandomString();
            Etag = RandomString();

            var lookup = new LookupDatabaseModel()
            {
                SomeValue = RandomString()
            };

            categoryIndex = new CategoryIndex<LookupDatabaseModel>()
            {
                Lookups = lookup.ToCollection()
            };
        }


        [Fact]
        public async Task GetAggregate_AggregateDoesNotExist_ReturnsNull()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            var result =
                await Sut.GetAggregateAsync(RandomString(),
                    CancellationToken.None);

            // ************ ASSERT *************

            result.Should().BeNull();
        }


        [Fact]
        public async Task UpsertAggregate_Commit_Get()
        {
            // ************ ARRANGE ************

            CreateUpsertAggregateData(
                out var aggregate,
                out var key,
                out var eTag);

            // ************ ACT ****************

            await Sut.UpsertAggregateAsync(eTag, aggregate,
                CancellationToken.None);

            await Sut.CommitTransactionAsync(CancellationToken.None);

            var result =
                await Sut.GetAggregateAsync(key, CancellationToken.None);

            // ************ ASSERT *************

            result!.Payload.SomeValue.Should().Be(aggregate.SomeValue);
        }


        [Fact]
        public async Task UpsertAggregate_UpdatesETagValue()
        {
            // ************ ARRANGE ************

            CreateUpsertAggregateData(
                out var aggregate,
                out var key,
                out var eTag);

            // ************ ACT ****************

            await Sut.UpsertAggregateAsync(eTag, aggregate,
                CancellationToken.None);

            await Sut.CommitTransactionAsync(CancellationToken.None);

            var result =
                await Sut.GetAggregateAsync(key, CancellationToken.None);

            // ************ ASSERT *************

            result!.Etag.Should().NotBe(eTag);
        }


        [Fact]
        public async Task GetCategoryIndex_DataDoesNotExist_ReturnNull()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            var result =
                await Sut.GetCategoryIndex(RandomString(),
                    CancellationToken.None);

            // ************ ASSERT *************

            result.Should().BeNull();
        }

        [Fact]
        public async Task UpsertCategoryIndex_Commit_Get()
        {
            // ************ ARRANGE ************

            CreateUpsertCategoryIndexData(
                out var index,
                out var key,
                out var eTag);

            // ************ ACT ****************

            await Sut.UpsertCategoryIndex(key, eTag, index,
                CancellationToken.None);

            await Sut.CommitTransactionAsync(CancellationToken.None);

            var result =
                await Sut.GetCategoryIndex(key, CancellationToken.None);

            // ************ ASSERT *************

            result!.Payload.Lookups.ShouldBeEquivalent(index.Lookups, (x, y) =>
                x.SomeValue == y.SomeValue);
        }

        [Fact]
        public async Task UpsertCategoryIndex_UpdatesETagValue()
        {
            // ************ ARRANGE ************

            CreateUpsertCategoryIndexData(
                out var index,
                out var key,
                out var eTag);

            // ************ ACT ****************

            await Sut.UpsertCategoryIndex(key, eTag, index,
                CancellationToken.None);

            await Sut.CommitTransactionAsync(CancellationToken.None);

            var result =
                await Sut.GetCategoryIndex(key, CancellationToken.None);

            // ************ ASSERT *************

            result!.Etag.Should().NotBe(eTag);
        }
    }
}