using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Api.Api;
using Moq;
using Testing.Common.Types;

namespace Testing.Common.Mocks
{
    public class LookupMapperMock
    {
        public LookupMapperMock()
        {
            _moq = new Mock<ILookupMapper<LookupDatabaseModel, Lookup>>();

            MapReturns = new();

            _moq.Setup(s=>
                s.Map(It.IsAny<LookupDatabaseModel>()))
                .Returns(MapReturns);
        }

        private readonly Mock<ILookupMapper<LookupDatabaseModel, Lookup>> _moq;

        public ILookupMapper<LookupDatabaseModel, Lookup> Object => _moq.Object;

        public Lookup MapReturns { get; }

        public void VerifyMap(LookupDatabaseModel databaseModel)
        {
            _moq.Verify(s=>
                s.Map(databaseModel));
        }
    }
}
