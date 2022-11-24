using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Api;
using Moq;
using Testing.Common.Types;

namespace Testing.Common.Mocks
{
    public class LookupMapperMock
    {
        public LookupMapperMock()
        {
            _moq = new Mock<ILookupMapper<LookupDatabaseModel, Lookup>>();

           
        }

        private readonly Mock<ILookupMapper<LookupDatabaseModel, Lookup>> _moq;

        public ILookupMapper<LookupDatabaseModel, Lookup> Object => _moq.Object;

      

        public void VerifyMap(LookupDatabaseModel databaseModel)
        {
            _moq.Verify(s=>
                s.Map(databaseModel));
        }

        public void Setup(LookupDatabaseModel forInput, out Lookup returns)
        {
            returns = new();

            _moq.Setup(s=>s.Map(forInput)).Returns(returns);
        }

    }
}
