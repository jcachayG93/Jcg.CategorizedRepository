using Support.DataModelRepository.Strategies;
using Support.DataModelRepository.Strategies.imp;
using Testing.Common.Mocks;
using Testing.Common.Types;

namespace Support.DataModelRepository.UnitTests.Strategies
{
    public class CommitStrategyTests
    {
        public CommitStrategyTests()
        {
            UnitOfWork = new();

            Sut = new(UnitOfWork.Object);
        }

        private UnitOfWorkMock UnitOfWork { get; }

        private CommitStrategy<AggregateDatabaseModel, LookupDatabaseModel> Sut
        {
            get;
        }


        [Fact]
        public async Task CommitChanges_DelegatesToUnitOfWork()
        {
            // ************ ARRANGE ************

            var cs = new CancellationToken();

            // ************ ACT ****************

            await Sut.CommitChangesAsync(cs);

            // ************ ASSERT *************

            UnitOfWork.VerifyCommitChanges(cs);
        }
    }
}