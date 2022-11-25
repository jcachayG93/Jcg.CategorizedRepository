using Moq;
using Support.DataModelRepository.Strategies;
using Testing.Common.Types;

namespace Support.DataModelRepository.UnitTests.TestCommon;

internal class UpsertAggregateStatregyMock
{
    public UpsertAggregateStatregyMock()
    {
        _moq = new();
    }

    public IUpsertAggregateStrategy<AggregateDatabaseModel,
        LookupDatabaseModel> Object => _moq.Object;

    public void VerifyUpsert(AggregateDatabaseModel aggregate)
    {
        _moq.Verify(s =>
            s.UpsertAsync(aggregate, AnyCt()));
    }

    private readonly Mock<IUpsertAggregateStrategy<AggregateDatabaseModel,
        LookupDatabaseModel>> _moq;
}