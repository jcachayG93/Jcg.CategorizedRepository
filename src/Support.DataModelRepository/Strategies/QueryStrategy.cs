﻿using Common.Api;
using Support.UnitOfWork;
using Support.UnitOfWork.Api;

namespace Support.DataModelRepository.Strategies;

internal class QueryStrategy<TAggregateDatabaseModel, TLookupDatabaseModel>
    : IQueryStrategy<TAggregateDatabaseModel, TLookupDatabaseModel>
    where TAggregateDatabaseModel : class, IAggregateDataModel
    where TLookupDatabaseModel : IRepositoryLookup
{
    public QueryStrategy(
        IUnitOfWork<TAggregateDatabaseModel, TLookupDatabaseModel>
            unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public Task<TAggregateDatabaseModel?> GetAggregateAsync(Guid key,
        CancellationToken cancellationToken)
    {
        return _unitOfWork.GetAggregateAsync(key.ToString(), cancellationToken);
    }

    /// <inheritdoc />
    public Task<CategoryIndex<TLookupDatabaseModel>> LookupNonDeletedAsync(
        CancellationToken cancellationToken)
    {
        return _unitOfWork.GetNonDeletedItemsCategoryIndex(cancellationToken);
    }

    /// <inheritdoc />
    public Task<CategoryIndex<TLookupDatabaseModel>> LookupDeletedAsync(
        CancellationToken cancellationToken)
    {
        return _unitOfWork.GetDeletedItemsCategoryIndex(cancellationToken);
    }

    private readonly IUnitOfWork<TAggregateDatabaseModel, TLookupDatabaseModel>
        _unitOfWork;
}