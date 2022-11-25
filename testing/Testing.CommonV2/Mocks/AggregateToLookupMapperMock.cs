using Jcg.DataAccessRepositories;
using Moq;
using Testing.Common.Types;

namespace Testing.Common.Mocks
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
            LookupDatabaseModel> Object => _moq.Object;

        public LookupDatabaseModel Returns { get; }

        public void VerifyToLookup(AggregateDatabaseModel aggregate)
        {
            _moq.Verify(s =>
                s.ToLookup(aggregate));
        }

        private readonly Mock<IAggregateToLookupMapper<AggregateDatabaseModel,
            LookupDatabaseModel>> _moq;
    }
}