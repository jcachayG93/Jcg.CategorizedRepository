using Support.DataModelRepository.Strategies;
using Support.DataModelRepository.UnitTests.TestCommon;
using Testing.Common.Mocks;
using Testing.Common.Types;

namespace Support.DataModelRepository.UnitTests
{
    public class InitializeCategoryIndexStrategyTests
    {
        public InitializeCategoryIndexStrategyTests()
        {
            CategoryIndexFactory = new();
            UnitOfWork = new();
            Sut = new(
                CategoryIndexFactory.Object, UnitOfWork.Object);
        }

        private CategoryIndexFactoryMock CategoryIndexFactory { get; }

        private UnitOfWorkMock UnitOfWork { get; }

        private InitializeCategoryIndexStrategy<AggregateDatabaseModel,
            LookupDatabaseModel> Sut { get; }
    }
}