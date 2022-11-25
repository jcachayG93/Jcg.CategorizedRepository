using Jcg.CategorizedRepository.DataModelRepo.Strategies.imp;
using Testing.CommonV2.Mocks;
using Testing.CommonV2.Types;

namespace Jcg.CategorizedRepository.UnitTests.DataModelRepo.Strategies
{
    public class CommitStrategyTests
    {
        public CommitStrategyTests()
        {
            UnitOfWork = new();

            Sut = new(UnitOfWork.Object);
        }

        private UnitOfWorkMock UnitOfWork { get; }

        private CommitStrategy<AggregateDatabaseModel, Lookup> Sut
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