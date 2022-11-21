using FluentAssertions;
using Support.UnitOfWork.Cache.Imp;
using Support.UnitOfWork.UnitTests.TestCommon;
using Testing.Common.Types;

namespace Support.UnitOfWork.UnitTests.Cache
{
    public class AggregatesCacheManagerTests
    {
        public AggregatesCacheManagerTests()
        {
            DbClient = new();

            Cache = new();

            Sut = new(DbClient.Object, Cache.Object);
        }

        private TransactionalDatabaseClientMock DbClient { get; }

        private CacheMock<AggregateDatabaseModel> Cache { get; }

        private AggregatesCacheManager<AggregateDatabaseModel,
            LookupDatabaseModel> Sut { get; }


        [Fact]
        public async Task
            Get_CacheHasKey_ReturnsResultFromCache_DoesNotReadTheDatabase()
        {
            // ************ ARRANGE ************

            Cache.SetupHasKey(true);

            var key = RandomString();

            // ************ ACT ****************

            var result = await Sut.GetAsync(key);

            // ************ ASSERT *************

            Cache.VerifyHasKey(key);

            result.Should().Be(Cache.GetReturns);

            DbClient.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            Get_CacheDoesNotHaveKey_ReadsDataFromDatabase_AddsResultToCache_ReturnsCachedData()
        {
            // ************ ARRANGE ************

            Cache.SetupHasKey(false);

            var key = RandomString();

            // ************ ACT ****************

            var result = await Sut.GetAsync(key);

            // ************ ASSERT *************

            Cache.VerifyHasKey(key);

            DbClient.VerifyGetAggregate(key);

            Cache.VerifyAdd(key, DbClient.GetAggregateReturns);

            result.Should().Be(Cache.GetReturns);
        }


        [Fact]
        public async Task
            Upsert_KeyNotInCache_ReadsDataFromDatabase_AddsItToCache()
        {
            // ************ ARRANGE ************

            Cache.SetupHasKey(false);

            var key = RandomString();

            var data = RandomAggregateDatabaseModel();

            // ************ ACT ****************

            await Sut.UpsertAsync(key, data);

            // ************ ASSERT *************

            DbClient.VerifyGetAggregate(key);

            Cache.VerifyAdd(key, DbClient.GetAggregateReturns);
        }


        [Fact]
        public async Task Upsert_DataInCache_DoesNotReadDatabase()
        {
            // ************ ARRANGE ************

            Cache.SetupHasKey(true);

            // ************ ACT ****************

            await Sut.UpsertAsync(RandomString(),
                RandomAggregateDatabaseModel());

            // ************ ASSERT *************

            DbClient.VerifyNoOtherCalls();
        }


        [Fact]
        public async Task Upsert_DelegatesToCacheUpsert_PassingCachedData()
        {
            // ************ ARRANGE ************

            Cache.SetupHasKey(true);

            var key = RandomString();


            // ************ ACT ****************

            await Sut.UpsertAsync(key, Cache.GetReturns);

            // ************ ASSERT *************

            Cache.VerifyUpsert(key, Cache.GetReturns);
        }

        [Fact]
        public void GetUpsertedItems_DelegatesToCache()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            var items = Sut.UpsertedItems;

            // ************ ASSERT *************

            items.Should().BeSameAs(Cache.UpsertedItemsReturns);
        }
    }
}