using Moq;
using Support.UnitOfWork.Api;
using Support.UnitOfWork.Cache;
using Testing.Common.Types;

namespace Support.UnitOfWork.UnitTests.TestCommon
{
    internal class CategoryIndexCacheManagerMock
    {
        public CategoryIndexCacheManagerMock()
        {
            _moq = new();

            UpsertedItemReturns = new(RandomString(),
                RandomString(), new());

            GetReturns = new();

            _moq.Setup(s => s.UpsertedItem)
                .Returns(UpsertedItemReturns);

            _moq.Setup(s => s.GetAsync().Result)
                .Returns(GetReturns);

            SetupIndexExist(false);
        }

        public ICategoryIndexCacheManager<LookupDatabaseModel> Object =>
            _moq.Object;

        public UpsertedItem<CategoryIndex<LookupDatabaseModel>>
            UpsertedItemReturns { get; }

        public CategoryIndex<LookupDatabaseModel> GetReturns { get; }

        public void VerifyUpsert(
            CategoryIndex<LookupDatabaseModel> categoryIndex)
        {
            _moq.Verify(s =>
                s.UpsertAsync(categoryIndex));
        }

        public void SetupIndexExist(bool returns)
        {
            _moq.Setup(s => s.IndexExistsAsync().Result)
                .Returns(returns);
        }

        private readonly Mock<ICategoryIndexCacheManager<LookupDatabaseModel>>
            _moq;
    }
}