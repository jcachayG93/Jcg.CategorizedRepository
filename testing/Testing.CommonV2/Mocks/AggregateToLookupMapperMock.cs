using Jcg.CategorizedRepository.Api;
using Moq;
using Testing.CommonV2.Types;

namespace Testing.CommonV2.Mocks
{
    public class AggregateToLookupMapperMock
    {
        public AggregateToLookupMapperMock()
        {
            _moq = new();

            Returns = new()
            {
                Key = RandomString()
            };

            _moq.Setup(s =>
                    s.ToLookup(It.IsAny<AggregateDatabaseModel>()))
                .Returns(Returns);
        }


        public IAggregateToLookupMapper<AggregateDatabaseModel,
            Lookup> Object => _moq.Object;

        public Lookup Returns { get; }

        public void VerifyToLookup(AggregateDatabaseModel aggregate)
        {
            _moq.Verify(s =>
                s.ToLookup(aggregate));
        }

        private readonly Mock<IAggregateToLookupMapper<AggregateDatabaseModel,
            Lookup>> _moq;
    }
}