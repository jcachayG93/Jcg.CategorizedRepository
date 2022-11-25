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

    public void VerifyUpsert(string key, AggregateDatabaseModel aggregate)
    {
        _moq.Verify(s =>
            s.UpsertAsync(key, aggregate, AnyCt()));
    }

    private readonly Mock<IUpsertAggregateStrategy<AggregateDatabaseModel,
        Lookup>> _moq;
}