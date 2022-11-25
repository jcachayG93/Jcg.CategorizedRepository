using Jcg.CategorizedRepository.DataModelRepo.Strategies;
using Moq;
using Testing.CommonV2.Types;

namespace Jcg.CategorizedRepository.UnitTests.DataModelRepo.TestCommon;

internal class UpsertAggregateStatregyMock
{
    public UpsertAggregateStatregyMock()
    {
        _moq = new();
    }

    public IUpsertAggregateStrategy<AggregateDatabaseModel,
        Lookup> Object => _moq.Object;

    public void VerifyUpsert(AggregateDatabaseModel aggregate)
    {
        _moq.Verify(s =>
            s.UpsertAsync(aggregate, AnyCt()));
    }

    private readonly Mock<IUpsertAggregateStrategy<AggregateDatabaseModel,
        Lookup>> _moq;
}