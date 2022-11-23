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

        private AggregateDatabaseModel RandomPayload()
        {
            return new();
        }


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

            var data = RandomAggregateDatabaseModel();

            // ************ ACT ****************

            await Sut.UpsertAsync(data);

            // ************ ASSERT *************

            DbClient.VerifyGetAggregate(data.Key);

            Cache.VerifyAdd(data.Key, DbClient.GetAggregateReturns);
        }


        [Fact]
        public async Task Upsert_KeyInCache_DoesNotReadDatabase()
        {
            // ************ ARRANGE ************

            Cache.SetupHasKey(true);

            // ************ ACT ****************

            await Sut.UpsertAsync(
                RandomAggregateDatabaseModel());

            // ************ ASSERT *************

            DbClient.VerifyNoOtherCalls();
        }


        [Fact]
        public async Task Upsert_DelegatesToCacheUpsert_PassingData()
        {
            // ************ ARRANGE ************

            Cache.SetupHasKey(true);


            var data = RandomPayload();


            // ************ ACT ****************

            await Sut.UpsertAsync(data);

            // ************ ASSERT *************

            Cache.VerifyUpsert(data.Key, data);
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