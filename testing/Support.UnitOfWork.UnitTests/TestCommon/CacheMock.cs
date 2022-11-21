using Moq;
using Support.UnitOfWork.Api;
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

        private void SetupGet(TData? returns)
        {
            _moq.Setup(s =>
                s.Get(AnyString())).Returns(returns);
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