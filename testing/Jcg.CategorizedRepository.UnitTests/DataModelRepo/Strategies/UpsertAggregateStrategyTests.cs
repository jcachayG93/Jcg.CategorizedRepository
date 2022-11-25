using Support.DataModelRepository.Strategies;
using Support.DataModelRepository.Strategies.imp;
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


        [Fact]
        public async Task Upsert_UpsertsAggregateToIndexManipulator()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            await Sut.UpsertAsync(Aggregate, CancellationToken.None);

            // ************ ASSERT *************

            IndexManipulator.VerifyUpsert(
                UnitOfWork.GetNonDeletedItemsCategoryIndexReturns, Aggregate);
        }


        [Fact]
        public async Task Upsert_UpsertsAggregateToUnitOfWork()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            await Sut.UpsertAsync(Aggregate, CancellationToken.None);

            // ************ ASSERT *************

            UnitOfWork.VerifyUpsertAggregate(Aggregate);
        }

        [Fact]
        public async Task Upsert_UpsertsNonDeletedCategoryIndexToUnitOfWork()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            await Sut.UpsertAsync(Aggregate, CancellationToken.None);

            // ************ ASSERT *************

            UnitOfWork.VerifyUpsertNonDeletedItemsCategoryIndex(UnitOfWork
                .GetNonDeletedItemsCategoryIndexReturns);
        }
    }
}