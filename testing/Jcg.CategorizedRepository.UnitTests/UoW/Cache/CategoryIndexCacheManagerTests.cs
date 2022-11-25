using FluentAssertions;
using Jcg.CategorizedRepository.Api;
using Jcg.CategorizedRepository.Api.Exceptions;
using Jcg.CategorizedRepository.UnitTests.UoW.TestCommon;
using Jcg.CategorizedRepository.UoW.Cache.Imp;
using Testing.CommonV2.Types;

namespace Jcg.CategorizedRepository.UnitTests.UoW.Cache
{
    public class CategoryIndexCacheManagerTests
    {
        public CategoryIndexCacheManagerTests()
        {
            DbClient = new();

            Cache = new();

            CategoryKey = RandomString();

            Sut = new(CategoryKey,
                DbClient.Object, Cache.Object);
        }

        private TransactionalDatabaseClientMock DbClient { get; }

        private CacheMock<CategoryIndex<Lookup>> Cache { get; }

        private CategoryIndexCacheManager<AggregateDatabaseModel,
                Lookup>
            Sut { get; }

        private string CategoryKey { get; }


        [Fact]
        public async Task
            Get_CacheHasKey_ReturnsResultFromCache_DoesNotReadFromDatabase()
        {
            // ************ ARRANGE ************

            Cache.SetupHasKey(true);

            // ************ ACT ****************

            var result = await Sut.GetAsync();

            // ************ ASSERT *************

            result.Should().Be(Cache.GetReturns);

            DbClient.VerifyNoOtherCalls();
        }


        [Fact]
        public async Task
            Get_CacheDoesNotHaveKey_ReadsFromDatabase_DatabaseReturnsNull_Throws()
        {
            // ************ ARRANGE ************

            Cache.SetupHasKey(false);
            DbClient.SetupGetCategoryIndexReturnsNull();

            // ************ ACT ****************

            Func<Task> fun = async () => { await Sut.GetAsync(); };

            // ************ ASSERT *************

            await fun.Should()
                .ThrowAsync<CategoryIndexIsUninitializedException>();
        }


        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public async Task
            IndexExist_CacheDoesNotHaveKey_ReadsFromDatabase_AddsToCache(
                bool cacheHasKey, bool shouldLoadAndAdd)
        {
            // ************ ARRANGE ************

            Cache.SetupHasKey(cacheHasKey);

            // ************ ACT ****************

            await Sut.IndexExistsAsync();

            // ************ ASSERT *************

            if (shouldLoadAndAdd)
            {
                DbClient.VerifyGetCategoryIndex(CategoryKey);

                Cache.VerifyAdd(CategoryKey, DbClient.GetCategoryIndexReturns);
            }
            else
            {
                DbClient.VerifyNoOtherCalls();
            }
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public async Task IndexExists_TrueIfCacheGetIsNotNull(
            bool cacheReturnsNotNull, bool expectedResult)
        {
            // ************ ARRANGE ************

            Cache.SetupHasKey(true);

            if (cacheReturnsNotNull)
            {
                Cache.SetupGetReturnsNotNull();
            }
            else
            {
                Cache.SetupGetReturnsNull();
            }

            // ************ ACT ****************

            var result = await Sut.IndexExistsAsync();

            // ************ ASSERT *************

            result.Should().Be(expectedResult);
        }


        [Fact]
        public async Task
            Get_CacheDoesNotHaveKey_ReadsFromDatabase_AddsResultToCache_ReturnsCacheData()
        {
            // ************ ARRANGE ************

            Cache.SetupHasKey(false);

            // ************ ACT ****************

            var result = await Sut.GetAsync();

            // ************ ASSERT *************

            DbClient.VerifyGetCategoryIndex(CategoryKey);

            Cache.VerifyAdd(CategoryKey, DbClient.GetCategoryIndexReturns);

            result.Should().Be(Cache.GetReturns);
        }


        [Fact]
        public async Task
            Upsert_KeyNotInCache_ReadsDataFromDatabase_DatabaseReturnsNotNull_AddsToCache_UpsertsToCache()
        {
            // ************ ARRANGE ************

            Cache.SetupHasKey(false);

            var payload = RandomCategoryIndex();

            // ************ ACT ****************

            await Sut.UpsertAsync(payload);

            // ************ ASSERT *************

            DbClient.VerifyGetCategoryIndex(CategoryKey);

            Cache.VerifyAdd(CategoryKey, DbClient.GetCategoryIndexReturns);

            Cache.VerifyUpsert(CategoryKey, payload);
        }

        [Fact]
        public async Task
            Upsert_KeyNotInCache_ReadsDataFromDatabase_DatabaseReturnsNull_UpsertsToCache()
        {
            // ************ ARRANGE ************

            Cache.SetupHasKey(false);

            var payload = RandomCategoryIndex();

            DbClient.SetupGetCategoryIndexReturnsNull();

            // ************ ACT ****************

            await Sut.UpsertAsync(payload);

            // ************ ASSERT *************

            DbClient.VerifyGetCategoryIndex(CategoryKey);


            Cache.VerifyUpsert(CategoryKey, payload);
        }


        [Fact]
        public async Task Upsert_KeyInCache_UpsertsToCache_DoesNotReadDatabase()
        {
            // ************ ARRANGE ************

            Cache.SetupHasKey(true);

            var payload = RandomCategoryIndex();

            // ************ ACT ****************

            await Sut.UpsertAsync(payload);

            // ************ ASSERT *************

            Cache.VerifyUpsert(CategoryKey, payload);

            DbClient.VerifyNoOtherCalls();
        }


        [Fact]
        public async Task Upsert_DelegatesToCacheUpsert_PassingData()
        {
            // ************ ARRANGE ************

            Cache.SetupHasKey(true);

            var data = RandomCategoryIndex();

            // ************ ACT ****************

            await Sut.UpsertAsync(data);

            // ************ ASSERT *************

            Cache.VerifyUpsert(CategoryKey, data);
        }


        [Fact]
        public void GetUpsertedItem_NoUpsertedItemsInCache_ReturnsNull()
        {
            // ************ ARRANGE ************

            Cache.SetupUpsertedItemReturnEmpty();

            // ************ ACT ****************

            var result = Sut.UpsertedItem;

            // ************ ASSERT *************

            result.Should().BeNull();
        }


        [Fact]
        public void GetUpsertedItem_UpsertedItemsInCache_ReturnsFirst()
        {
            // ************ ARRANGE ************

            Cache.SetupUpsertedItemReturnsOne(out var expected);

            // ************ ACT ****************

            var result = Sut.UpsertedItem;

            // ************ ASSERT *************

            result.Should().Be(expected);
        }
    }
}