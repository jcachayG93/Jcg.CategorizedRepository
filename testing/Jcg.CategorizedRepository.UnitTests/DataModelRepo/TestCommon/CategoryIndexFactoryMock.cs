using Jcg.CategorizedRepository.Api;
using Jcg.CategorizedRepository.DataModelRepo.Support;
using Moq;
using Testing.CommonV2.Types;

namespace Jcg.CategorizedRepository.UnitTests.DataModelRepo.TestCommon
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