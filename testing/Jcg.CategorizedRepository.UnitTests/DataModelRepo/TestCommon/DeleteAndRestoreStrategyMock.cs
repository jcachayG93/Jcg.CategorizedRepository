using Jcg.CategorizedRepository.DataModelRepo.Strategies;
using Moq;
using Testing.CommonV2.Types;

namespace Jcg.CategorizedRepository.UnitTests.DataModelRepo.TestCommon
{
    internal class DeleteAndRestoreStrategyMock
    {
        public DeleteAndRestoreStrategyMock()
        {
            _moq = new();
        }

        public IDeleteAndRestoreStrategy<AggregateDatabaseModel,
            Lookup> Object => _moq.Object;

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
            Lookup>> _moq;
    }
}