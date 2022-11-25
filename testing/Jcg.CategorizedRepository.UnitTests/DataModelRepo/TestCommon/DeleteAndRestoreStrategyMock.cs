using Moq;
using Support.DataModelRepository.Strategies;
using Testing.Common.Types;

namespace Support.DataModelRepository.UnitTests.TestCommon
{
    internal class DeleteAndRestoreStrategyMock
    {
        public DeleteAndRestoreStrategyMock()
        {
            _moq = new();
        }

        public IDeleteAndRestoreStrategy<AggregateDatabaseModel,
            LookupDatabaseModel> Object => _moq.Object;

        public void VerifyDelete(Guid key)
        {
            _moq.Verify(s =>
                s.DeleteAsync(key, AnyCt()));
        }

        public void VerifyRestore(Guid key)
        {
            _moq.Verify(s =>
                s.RestoreAsync(key, AnyCt()));
        }

        private readonly Mock<IDeleteAndRestoreStrategy<AggregateDatabaseModel,
            LookupDatabaseModel>> _moq;
    }
}