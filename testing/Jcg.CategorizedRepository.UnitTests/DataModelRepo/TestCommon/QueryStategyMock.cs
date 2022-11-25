using Jcg.CategorizedRepository.Api;
using Jcg.CategorizedRepository.DataModelRepo.Strategies;
using Moq;
using Testing.CommonV2.Types;

namespace Jcg.CategorizedRepository.UnitTests.DataModelRepo.TestCommon;

internal class QueryStategyMock
{
    public QueryStategyMock()
    {
        _moq = new();

        GetAggregateReturns = RandomAggregateDatabaseModel();

        LookupNonDeletedReturns = RandomCategoryIndex();

        LookupDeletedReturns = RandomCategoryIndex();

        _moq.Setup(s =>
                s.GetAggregateAsync(AnyId(), AnyCt()).Result)
            .Returns(GetAggregateReturns);

        _moq.Setup(s =>
                s.LookupNonDeletedAsync(AnyCt()).Result)
            .Returns(LookupNonDeletedReturns);

        _moq.Setup(s =>
                s.LookupDeletedAsync(AnyCt()).Result)
            .Returns(LookupDeletedReturns);
    }

    public IQueryStrategy<AggregateDatabaseModel, LookupDatabaseModel>
        Object => _moq.Object;

    public AggregateDatabaseModel GetAggregateReturns { get; }

    public CategoryIndex<LookupDatabaseModel> LookupNonDeletedReturns { get; }

    public CategoryIndex<LookupDatabaseModel> LookupDeletedReturns { get; }

    public void VerifyGetAggregate(Guid key)
    {
        _moq.Verify(s => s.GetAggregateAsync(key, AnyCt()));
    }

    private readonly
        Mock<IQueryStrategy<AggregateDatabaseModel, LookupDatabaseModel>>
        _moq;
}