using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Api;
using Moq;
using Support.CategorizedRepository.Support;
using Testing.Common.Types;

namespace Support.CategorizedRepository.UnitTests.TestCommon
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
