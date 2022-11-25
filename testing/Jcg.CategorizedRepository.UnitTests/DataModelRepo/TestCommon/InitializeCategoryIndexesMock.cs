using Jcg.CategorizedRepository.DataModelRepo.Strategies;
using Moq;

namespace Jcg.CategorizedRepository.UnitTests.DataModelRepo.TestCommon
{
    internal class InitializeCategoryIndexesMock
    {
        public InitializeCategoryIndexesMock()
        {
            _moq = new();
        }

        public IInitializeCategoryIndexStrategy Object => _moq.Object;

        public void VerifyInitializeCategoryIndexes(
            CancellationToken cancellationToken)
        {
            _moq.Verify(s =>
                s.InitializeCategoryIndexes(cancellationToken));
        }

        private readonly Mock<IInitializeCategoryIndexStrategy> _moq;
    }
}