using Support.DataModelRepository.Strategies;
using Support.DataModelRepository.UnitTests.TestCommon;
using Testing.Common.Mocks;
using Testing.Common.Types;

namespace Support.DataModelRepository.UnitTests.Strategies
{
    public class UpsertAggregateStrategyTests
    {
        public UpsertAggregateStrategyTests()
        {
            IndexManipulator = new();
            UnitOfWork = new();
            Sut = new(IndexManipulator.Object, UnitOfWork.Object);
            Key = Guid.NewGuid();
            Aggregate = RandomAggregateDatabaseModel();
        }

        private CategoryIndexManipulatorMock IndexManipulator { get; }

        private UnitOfWorkMock UnitOfWork { get; }

        private UpsertAggregateStrategy<AggregateDatabaseModel,
            LookupDatabaseModel> Sut { get; }


        public AggregateDatabaseModel Aggregate { get; }


        [Fact]
        public async Task Upsert_GetsDeletedItemsCategoryIndex()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            await Sut.UpsertAsync(Aggregate, CancellationToken.None);

            // ************ ASSERT *************

            UnitOfWork.VerifyGetNonDeletedCategoryIndex();
        }


        [Fact(Skip = "not implemented")]
        public async Task Upsert_UpsertsAggregateToIndexManipulator()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            await Sut.UpsertAsync(Aggregate, CancellationToken.None);

            // ************ ASSERT *************

            IndexManipulator.VerifyUpsert(
                UnitOfWork.GetNonDeletedItemsCategoryIndexReturns, Aggregate);
        }


        [Fact(Skip = "not implemented")]
        public async Task Upsert_UpsertsAggregateToUnitOfWork()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            await Sut.UpsertAsync(Aggregate, CancellationToken.None);

            // ************ ASSERT *************

            // TODO: Continue here...
        }

        [Fact(Skip = "not implemented")]
        public async Task Upsert_UpsertsNonDeletedCategoryIndexToUnitOfWork()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            // ************ ASSERT *************
        }
    }
}