using Jcg.CategorizedRepository.Api;
using Jcg.CategorizedRepository.UoW.Cache;
using Moq;
using Testing.CommonV2.Types;

namespace Jcg.CategorizedRepository.UnitTests.UoW.TestCommon
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

        public ICategoryIndexCacheManager<Lookup> Object =>
            _moq.Object;

        public UpsertedItem<CategoryIndex<Lookup>>
            UpsertedItemReturns { get; }

        public CategoryIndex<Lookup> GetReturns { get; }

        public void VerifyUpsert(
            CategoryIndex<Lookup> categoryIndex)
        {
            _moq.Verify(s =>
                s.UpsertAsync(categoryIndex));
        }

        public void SetupIndexExist(bool returns)
        {
            _moq.Setup(s => s.IndexExistsAsync().Result)
                .Returns(returns);
        }

        private readonly Mock<ICategoryIndexCacheManager<Lookup>>
            _moq;
    }
}