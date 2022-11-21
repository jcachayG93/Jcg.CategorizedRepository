using FluentAssertions;
using Support.UnitOfWork.Api;
using Support.UnitOfWork.Api.Exceptions;
using Support.UnitOfWork.Cache.Imp;
using Support.UnitOfWork.UnitTests.TestCommon;
using Testing.Common.Types;

namespace Support.UnitOfWork.UnitTests.Cache
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

        private CacheMock<CategoryIndex<LookupDatabaseModel>> Cache { get; }

        private CategoryIndexCacheManager<AggregateDatabaseModel,
                LookupDatabaseModel>
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
            Upsert_KeyNotInCache_ReadsDataFromDatabase_DatabaseReturnsNull_Throws()
        {
            // ************ ARRANGE ************

            Cache.SetupHasKey(false);

            DbClient.SetupGetCategoryIndexReturnsNull();

            // ************ ACT ****************

            Func<Task> fun = async () =>
            {
                await Sut.UpsertAsync(RandomCategoryIndex());
            };

            // ************ ASSERT *************

            await fun.Should()
                .ThrowAsync<CategoryIndexIsUninitializedException>();
        }


        [Fact]
        public async Task
            Upsert_KeyNotInCche_ReadsDataFromDatabase_AddsResultToCache()
        {
            // ************ ARRANGE ************

            Cache.SetupHasKey(false);

            // ************ ACT ****************

            await Sut.UpsertAsync(RandomCategoryIndex());

            // ************ ASSERT *************

            DbClient.VerifyGetCategoryIndex(CategoryKey);

            Cache.VerifyAdd(CategoryKey, DbClient.GetCategoryIndexReturns);
        }


        [Fact]
        public async Task Upsert_KeyInCache_DoesNotReadDatabase()
        {
            // ************ ARRANGE ************

            Cache.SetupHasKey(true);

            // ************ ACT ****************

            await Sut.UpsertAsync(RandomCategoryIndex());

            // ************ ASSERT *************

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
        public async Task GetUpsertedItem_NoUpsertedItemsInCache_ReturnsNull()
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