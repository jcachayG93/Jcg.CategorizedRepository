﻿using Jcg.DataAccessRepositories;
using Moq;
using Testing.Common.Types;

namespace Testing.Common.Mocks;

public class AggregateMapperMock
{
    public AggregateMapperMock()
    {
        _moq = new Mock<IAggregateMapper<Aggregate, AggregateDatabaseModel>>();

        ToAggregateReturns = new ();

        ToDatabaseModelReturns = new();

        _moq.Setup(s=>s.ToAggregate(It.IsAny<AggregateDatabaseModel>()))
            .Returns(ToAggregateReturns);

        _moq.Setup(s=>s.ToDatabaseModel(It.IsAny<Aggregate>()))
            .Returns(ToDatabaseModelReturns);
    }

    private readonly Mock<IAggregateMapper<Aggregate, AggregateDatabaseModel>> _moq;

    public IAggregateMapper<Aggregate, AggregateDatabaseModel> Object => _moq.Object;

    public Aggregate ToAggregateReturns { get; }

    public AggregateDatabaseModel ToDatabaseModelReturns { get; }

    public void VerifyToAggregate(AggregateDatabaseModel databaseModel)
    {
        _moq.Verify(s=>s.ToAggregate(databaseModel));
    }

    public void VerifyToDatabaseModel(Aggregate aggregate)
    {
        _moq.Verify(s=>s.ToDatabaseModel(aggregate));
    }
}