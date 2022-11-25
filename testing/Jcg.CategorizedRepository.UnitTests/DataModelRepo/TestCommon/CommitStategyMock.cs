using Moq;
using Support.DataModelRepository.Strategies;

namespace Support.DataModelRepository.UnitTests.TestCommon
{
    internal class CommitStategyMock
    {
        public CommitStategyMock()
        {
            _moq = new();
        }

        public ICommitStrategy Object => _moq.Object;

        public void VerifyCommit(CancellationToken cancellationToken)
        {
            _moq.Verify(s => s.CommitChangesAsync(cancellationToken));
        }

        private readonly Mock<ICommitStrategy> _moq;
    }
}