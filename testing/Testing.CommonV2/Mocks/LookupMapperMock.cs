using Jcg.CategorizedRepository.Api;
using Moq;
using Testing.CommonV2.Types;

namespace Testing.CommonV2.Mocks
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
