using FluentAssertions;
using Jcg.CategorizedRepository.DataModelRepo.Strategies.imp;
using Testing.CommonV2.Mocks;
using Testing.CommonV2.Types;

namespace Jcg.CategorizedRepository.UnitTests.DataModelRepo.Strategies
{
    public class QueryStrategyTests
    {
        public QueryStrategyTests()
        {
            UnitOfWork = new();

            Sut = new(UnitOfWork.Object);
        }

        private UnitOfWorkMock UnitOfWork { get; }

        private QueryStrategy<AggregateDatabaseModel, LookupDatabaseModel> Sut
        {
            get;
        }


        [Fact]
        public async Task GetAggregate_DelegatesToUnitOfWork()
        {
            // ************ ARRANGE ************

            var key = Guid.NewGuid();

            // ************ ACT ****************

            var result =
                await Sut.GetAggregateAsync(key, CancellationToken.None);

            // ************ ASSERT *************

            UnitOfWork.VerifyGetAggregate(key.ToString());

            result.Should().Be(UnitOfWork.GetAggregateReturns);
        }


        [Fact]
        public async Task LookupNonDeletedAggregates_DelegatesToUnitOfWork()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            var result =
                await Sut.LookupNonDeletedAsync(CancellationToken.None);

            // ************ ASSERT *************

            result.Should()
                .BeSameAs(UnitOfWork.GetNonDeletedItemsCategoryIndexReturns);
        }


        [Fact]
        public async Task LookupDeletedAggregates_DelegatesToUnitOfWork()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            var result = await Sut.LookupDeletedAsync(CancellationToken.None);

            // ************ ASSERT *************

            result.Should()
                .BeSameAs(UnitOfWork.GetDeletedItemsCategoryIndexReturns);
        }
    }
}