using Common.Api;
using Moq;
using Support.DataModelRepository.Support;
using Testing.Common.Types;

namespace Support.DataModelRepository.UnitTests.TestCommon
{
    internal class CategoryIndexFactoryMock
    {
        public CategoryIndexFactoryMock()
        {
            _moq = new();

            Returns = RandomCategoryIndex();

            _moq.Setup(s => s.Create())
                .Returns(Returns);
        }

        public CategoryIndexFactory<LookupDatabaseModel> Object => _moq.Object;

        public CategoryIndex<LookupDatabaseModel> Returns { get; }

        private readonly Mock<CategoryIndexFactory<LookupDatabaseModel>> _moq;
    }
}