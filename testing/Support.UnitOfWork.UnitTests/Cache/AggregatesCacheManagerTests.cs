using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Support.UnitOfWork.UnitTests.Cache
{
    public class AggregatesCacheManagerTests
    {
        [Fact(Skip = "Not Implemented")]
        public async Task Get_CacheHasKey_ReturnsResultFromCache_DoesNotReadTheDatabase()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            // ************ ASSERT *************
        }

        [Fact(Skip = "Not Implemented")]
        public async Task Get_CacheDoesNotHasKey_ReadsDataFromDatabase_AddsResultToCache_ReturnsCachedData()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            // ************ ASSERT *************
        }


        [Fact(Skip = "Not Implemented")]
        public async Task Upsert_KeyNotInCache_ReadsDataFromDatabase_AddsItToCache()
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
