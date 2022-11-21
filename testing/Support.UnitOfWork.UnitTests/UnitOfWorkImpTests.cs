using FluentAssertions;
using Support.UnitOfWork.UnitTests.TestCommon;
using Testing.Common.Types;

namespace Support.UnitOfWork.UnitTests
{
    public class UnitOfWorkImpTests
    {
        public UnitOfWorkImpTests()
        {
            DbClient = new();

            AggregatesCache = new();

            DeletedItemsCategoryIndexCache = new();

            NonDeletedItemsCategoryIndexCache = new();


            Sut = new(
                DbClient.Object, AggregatesCache.Object,
                DeletedItemsCategoryIndexCache.Object,
                NonDeletedItemsCategoryIndexCache.Object);
        }


        private TransactionalDatabaseClientMock DbClient { get; }

        private AggregatesCacheManagerMock AggregatesCache { get; }

        private CategoryIndexCacheManagerMock DeletedItemsCategoryIndexCache
        {
            get;
        }

        private CategoryIndexCacheManagerMock NonDeletedItemsCategoryIndexCache
        {
            get;
        }

        private UnitOfWorkImp<AggregateDatabaseModel, LookupDatabaseModel> Sut
        {
            get;
        }


        [Fact]
        public async Task
            GetNonDeletedCategoryItems_DelegatesToNonDeletedCategoryItemsCache()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            var result =
                await Sut.GetNonDeletedItemsCategoryIndex(
                    CancellationToken.None);

            // ************ ASSERT *************

            result.Should().Be(NonDeletedItemsCategoryIndexCache.GetReturns);
        }


        [Fact]
        public async Task
            GetDeletedItemsCategoryIndex_DelegatesToDeletedCategoryItemsCache()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            var result =
                await Sut.GetDeletedItemsCategoryIndex(
                    CancellationToken.None);

            // ************ ASSERT *************

            result.Should()
                .Be(DeletedItemsCategoryIndexCache.GetReturns);
        }


        [Fact]
        public async Task
            UpsertDeletedItemsCategoryIndex_DelegatesToDeletedCategoryItemsCache()
        {
            // ************ ARRANGE ************

            var categoryIndex = RandomCategoryIndex();

            // ************ ACT ****************

            await Sut.UpsertDeletedItemsCategoryIndex(
                categoryIndex, CancellationToken.None);

            // ************ ASSERT *************


            DeletedItemsCategoryIndexCache
                .VerifyUpsert(categoryIndex);
        }


        [Fact]
        public async Task
            UpsertNonDeletedItemsCategoryIndex_DelegatesToNonDeletedCategoryItemsCache()
        {
            // ************ ARRANGE ************

            var categoryIndex = RandomCategoryIndex();

            // ************ ACT ****************

            await Sut.UpsertNonDeletedItemsCategoryIndex(categoryIndex,
                CancellationToken.None);

            // ************ ASSERT *************

            NonDeletedItemsCategoryIndexCache
                .VerifyUpsert(categoryIndex);
        }


        [Fact]
        public async Task GetAggregate_DelegatesToAggregatesCache()
        {
            // ************ ARRANGE ************

            var key = RandomString();

            // ************ ACT ****************

            var result =
                await Sut.GetAggregateAsync(key, CancellationToken.None);

            // ************ ASSERT *************

            AggregatesCache.VerifyGet(key);

            result.Should().Be(AggregatesCache.GetReturns);
        }


        [Fact]
        public async Task UpsertAggregate_DelegatesToAggregatesCache()
        {
            // ************ ARRANGE ************

            var key = RandomString();

            var aggregate = RandomAggregateDatabaseModel();

            // ************ ACT ****************

            await Sut.UpsertAggregateAsync(key, aggregate,
                CancellationToken.None);

            // ************ ASSERT *************

            AggregatesCache.VerifyUpsert(key, aggregate);
        }

        [Fact]
        public async Task
            CommitChanges_DelegatesToUpsertInDatabaseClient_WithAggregates_AndCategoryIndexes_Commits()
        {
            // ************ ARRANGE ************

            AggregatesCache.SetupUpsertedItemsReturns(5);

            // ************ ACT ****************

            await Sut.CommitChangesAsync(CancellationToken.None);

            // ************ ASSERT *************

            DbClient.VerifyUpsertCategoryIndex(
                NonDeletedItemsCategoryIndexCache.UpsertedItemReturns.Key,
                NonDeletedItemsCategoryIndexCache.UpsertedItemReturns.ETag,
                NonDeletedItemsCategoryIndexCache.UpsertedItemReturns.PayLoad);

            DbClient.VerifyUpsertCategoryIndex(
                DeletedItemsCategoryIndexCache.UpsertedItemReturns.Key,
                DeletedItemsCategoryIndexCache.UpsertedItemReturns.ETag,
                DeletedItemsCategoryIndexCache.UpsertedItemReturns.PayLoad);

            foreach (var aggregate in AggregatesCache.UpsertedItemsReturns)
            {
                DbClient.VerifyUpsertAggregate(aggregate.Key, aggregate.ETag,
                    aggregate.PayLoad);
            }

            DbClient.VerifyCommitTransaction();
        }
    }
}