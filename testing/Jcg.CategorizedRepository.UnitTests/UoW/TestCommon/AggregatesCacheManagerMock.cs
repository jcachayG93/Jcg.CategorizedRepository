using Moq;
using Support.UnitOfWork.Cache;
using Testing.Common.Types;

namespace Support.UnitOfWork.UnitTests.TestCommon
{
    internal class AggregatesCacheManagerMock
    {
        public AggregatesCacheManagerMock()
        {
            _moq = new();

            SetupUpsertedItemsReturns(1);

            GetReturns = new();

            _moq.Setup(s => s.GetAsync(AnyString()).Result)
                .Returns(GetReturns);
        }

        public IAggregatesCacheManager<AggregateDatabaseModel> Object =>
            _moq.Object;

        public AggregateDatabaseModel GetReturns { get; }

        public IEnumerable<UpsertedItem<AggregateDatabaseModel>>
            UpsertedItemsReturns { get; private set; }

        public void VerifyGet(string key)
        {
            _moq.Verify(s => s.GetAsync(key));
        }


        public void VerifyUpsert(AggregateDatabaseModel aggregate)
        {
            _moq.Verify(s =>
                s.UpsertAsync(aggregate));
        }

        public void SetupUpsertedItemsReturns(int count)
        {
            UpsertedItemsReturns = Enumerable.Range(0, count)
                .Select(i => RandomUpsertedItem()).ToList();

            _moq.Setup(s =>
                s.UpsertedItems).Returns(UpsertedItemsReturns);
        }

        private UpsertedItem<AggregateDatabaseModel> RandomUpsertedItem()
        {
            return new(RandomString(), RandomString(), new());
        }

        private readonly Mock<IAggregatesCacheManager<AggregateDatabaseModel>>
            _moq;
    }
}