using Jcg.CategorizedRepository.Api;
using Jcg.CategorizedRepository.CategorizedRepo.Support;
using Moq;
using Testing.CommonV2.Types;

namespace Jcg.CategorizedRepository.UnitTests.CategorizedRepo.TestCommon
{

    internal class LookupMapperAdapterMock
    {
        public LookupMapperAdapterMock()
        {
            _moq = new Mock<ILookupMapperAdapter<LookupDatabaseModel, Lookup>>();

            MapReturns = new List<Lookup>();

            _moq.Setup(s => s.Map(It.IsAny<CategoryIndex<LookupDatabaseModel>>()))
                .Returns(MapReturns);
        }

        private readonly Mock<ILookupMapperAdapter<LookupDatabaseModel, Lookup>> _moq;

        public ILookupMapperAdapter<LookupDatabaseModel, Lookup> Object => _moq.Object;

        public void VerifyMap(CategoryIndex<LookupDatabaseModel> lookups)
        {
            _moq.Verify(s=>s.Map(lookups));
        }

        public IEnumerable<Lookup> MapReturns { get; }
    }
}
