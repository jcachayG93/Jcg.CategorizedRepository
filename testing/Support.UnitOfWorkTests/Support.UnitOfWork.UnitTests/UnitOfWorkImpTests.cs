using Common.Api.Exceptions;
using FluentAssertions;
using Support.UnitOfWork.Api.Exceptions;
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

        private UnitOfWork<AggregateDatabaseModel, LookupDatabaseModel> Sut
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

        [Fact]
        public async Task Commit_MoreThanOnce_Throws()
        {
            // ************ ARRANGE ************

            await Sut.UpsertAggregateAsync(RandomString(),
                RandomAggregateDatabaseModel(), CancellationToken.None);

            // ************ ACT ****************

            await Sut.CommitChangesAsync(CancellationToken.None);

            var fun = new Func<Task>(async () =>
            {
                await Sut.CommitChangesAsync(CancellationToken.None);
            });

            // ************ ASSERT *************

            await fun.Should()
                .ThrowAsync<UnitOfWorkWasAlreadyCommittedException>();
        }

        [Fact]
        public async Task DeletedCategoryIndex_Upsert_After_Commit_Throws()
        {
            // ************ ARRANGE ************

            var index = RandomCategoryIndex();

            await Sut.UpsertDeletedItemsCategoryIndex(index,
                CancellationToken.None);

            await Sut.CommitChangesAsync(CancellationToken.None);

            var fun = new Func<Task>(async () =>
            {
                await Sut.UpsertDeletedItemsCategoryIndex(index,
                    CancellationToken.None);
            });

            // ************ ASSERT *************

            await fun.Should()
                .ThrowAsync<UnitOfWorkWasAlreadyCommittedException>();
        }

        [Fact]
        public async Task NonDeletedCategoryIndex_Upsert_After_Commit_Throws()
        {
            // ************ ARRANGE ************

            var index = RandomCategoryIndex();

            await Sut.UpsertNonDeletedItemsCategoryIndex(index,
                CancellationToken.None);

            await Sut.CommitChangesAsync(CancellationToken.None);

            var fun = new Func<Task>(async () =>
            {
                await Sut.UpsertNonDeletedItemsCategoryIndex(index,
                    CancellationToken.None);
            });

            // ************ ASSERT *************

            await fun.Should()
                .ThrowAsync<UnitOfWorkWasAlreadyCommittedException>();
        }

        [Fact]
        public async Task UpsertAggregate_After_Commit_Throws()
        {
            // ************ ARRANGE ************

            var aggregate = RandomAggregateDatabaseModel();

            await Sut.UpsertAggregateAsync(RandomString(), aggregate,
                CancellationToken.None);

            await Sut.CommitChangesAsync(CancellationToken.None);


            // ************ ACT ****************

            var fun = new Func<Task>(async () =>
            {
                await Sut.UpsertAggregateAsync(RandomString(), aggregate,
                    CancellationToken.None);
            });

            // ************ ASSERT *************

            await fun.Should()
                .ThrowAsync<UnitOfWorkWasAlreadyCommittedException>();
        }

        [Fact]
        public async Task DeletedCategoryIndex_GetAfterCommit_WorksAsUsual()
        {
            // ************ ARRANGE ************


            await Sut.UpsertDeletedItemsCategoryIndex(RandomCategoryIndex(),
                CancellationToken.None);

            await Sut.CommitChangesAsync(CancellationToken.None);

            // ************ ACT ****************

            var result =
                await Sut.GetDeletedItemsCategoryIndex(CancellationToken.None);

            // ************ ASSERT *************

            result.Should().BeSameAs(DeletedItemsCategoryIndexCache.GetReturns);
        }

        [Fact]
        public async Task NonDeletedCategoryIndex_GetAfterCommit_WorksAsUsual()
        {
            // ************ ARRANGE ************


            await Sut.UpsertNonDeletedItemsCategoryIndex(RandomCategoryIndex(),
                CancellationToken.None);

            await Sut.CommitChangesAsync(CancellationToken.None);

            // ************ ACT ****************

            var result =
                await Sut.GetNonDeletedItemsCategoryIndex(
                    CancellationToken.None);

            // ************ ASSERT *************

            result.Should()
                .BeSameAs(NonDeletedItemsCategoryIndexCache.GetReturns);
        }

        [Fact]
        public async Task GetAggregate_AfterCommit_WorksAsUsual()
        {
            // ************ ARRANGE ************

            var aggregate = RandomAggregateDatabaseModel();

            await Sut.UpsertAggregateAsync(RandomString(), aggregate,
                CancellationToken.None);

            await Sut.CommitChangesAsync(CancellationToken.None);

            // ************ ACT ****************

            var result =
                await Sut.GetAggregateAsync(RandomString(),
                    CancellationToken.None);

            // ************ ASSERT *************

            result.Should().BeSameAs(AggregatesCache.GetReturns);
        }


        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public async Task
            CategoryIndexIsInitialized_DelegatesToDeletedIndexCache(
                bool cacheIndexExistsResult, bool expectedResult)
        {
            // ************ ARRANGE ************

            DeletedItemsCategoryIndexCache.SetupIndexExist(
                cacheIndexExistsResult);

            // ************ ACT ****************

            var result =
                await Sut.CheckIfDeletedCategoryIndexesExistsAsync(
                    CancellationToken.None);

            // ************ ASSERT *************

            result.Should().Be(expectedResult);
        }


        [Theory]
        [InlineData(true, true, false)]
        [InlineData(false, false, false)]
        [InlineData(true, false, true)]
        [InlineData(false, true, true)]
        public async Task
            CategoryIndexIsInitialized_OneIndexExistButTheOtherDont_Throws(
                bool deletedIndexExists, bool nonDeletedIndexExist,
                bool shouldThrow)
        {
            // ************ ARRANGE ************

            DeletedItemsCategoryIndexCache.SetupIndexExist(deletedIndexExists);

            NonDeletedItemsCategoryIndexCache.SetupIndexExist(
                nonDeletedIndexExist);

            // ************ ACT ****************

            Func<Task> fun = async () =>
            {
                await Sut.CheckIfDeletedCategoryIndexesExistsAsync(
                    CancellationToken
                        .None);
            };

            // ************ ASSERT *************

            if (shouldThrow)
            {
                await fun.Should()
                    .ThrowAsync<InternalRepositoryErrorException>();
            }
            else
            {
                await fun.Should().NotThrowAsync();
            }
        }
    }
}