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


        [Fact(Skip = "Not Implemented")]
        public async Task
            Get_CacheHasKey_ReturnsResultFromCache_DoesNotReadTheDatabase()
        {
            // ************ ARRANGE ************

            Cache.SetupHasKey(true);

            // ************ ACT ****************

            // ************ ASSERT *************
        }

        [Fact(Skip = "Not Implemented")]
        public async Task
            Get_CacheDoesNotHasKey_ReadsDataFromDatabase_AddsResultToCache_ReturnsCachedData()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            // ************ ASSERT *************
        }


        [Fact(Skip = "Not Implemented")]
        public async Task
            Upsert_KeyNotInCache_ReadsDataFromDatabase_AddsItToCache()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            // ************ ASSERT *************
        }

        [Fact(Skip = "Not Implemented")]
        public async Task Upsert_DelegatesToCacheUpsert()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            // ************ ASSERT *************
        }

        [Fact(Skip = "Not Implemented")]
        public async Task GetUpsertedItems_DelegatesToCache()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            // ************ ASSERT *************
        }
    }
}