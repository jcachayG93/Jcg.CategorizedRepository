using Jcg.DataAccessRepositories;
using Moq;
using Support.UnitOfWork.Cache;
using Support.UnitOfWork.Cache.Imp;

namespace Support.UnitOfWork.UnitTests.TestCommon
{
    internal class CacheMock<TData>
        where TData : class, new()

    {
        public CacheMock()
        {
            GetReturns = new();

            SetupGet(GetReturns);

            UpsertedItemsReturns =
                Enumerable.Range(0, 3)
                    .Select(i =>
                        new UpsertedItem<TData>(RandomString(), RandomString(),
                            new()))
                    .ToList();

            _moq.Setup(s =>
                s.UpsertedItems).Returns(UpsertedItemsReturns);

            SetupHasKey(true);
        }

        public Cache<TData> Object => _moq.Object;

        public TData GetReturns { get; }

        public IEnumerable<UpsertedItem<TData>> UpsertedItemsReturns { get; }

        public void VerifyHasKey(string key)
        {
            _moq.Verify(s => s.HasKey(key));
        }

        public void SetupHasKey(bool value)
        {
            _moq.Setup(s => s.HasKey(AnyString()))
                .Returns(value);
        }

        public void SetupUpsertedItemReturnEmpty()
        {
            SetupUpsertedItems();
        }

        public void SetupUpsertedItemReturnsOne(out UpsertedItem<TData> item)
        {
            item = new UpsertedItem<TData>(RandomString(), RandomString(),
                new());

            SetupUpsertedItems(item);
        }

        private void SetupUpsertedItems(
            params UpsertedItem<TData>[] upsertedItems)
        {
            _moq.Setup(s =>
                s.UpsertedItems).Returns(upsertedItems);
        }

        private void SetupGet(TData returns)
        {
            _moq.Setup(s =>
                s.Get(AnyString())).Returns(returns);
        }

        public void SetupGetReturnsNull()
        {
            SetupGet(null);
        }

        public void SetupGetReturnsNotNull()
        {
            SetupGet(new());
        }

        public void VerifyAdd(string key, IETagDto<TData> data)
        {
            _moq.Verify(s =>
                s.Add(key, data));
        }

        public void VerifyUpsert(string key, TData payload)
        {
            _moq.Verify(s => s.Upsert(key, payload));
        }

        private readonly Mock<Cache<TData>> _moq = new();
    }
}