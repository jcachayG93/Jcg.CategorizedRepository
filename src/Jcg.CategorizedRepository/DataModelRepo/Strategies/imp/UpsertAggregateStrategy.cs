﻿using Jcg.CategorizedRepository.Api;
using Jcg.CategorizedRepository.DataModelRepo.Support.IndexManipulator;
using Jcg.CategorizedRepository.UoW;

namespace Jcg.CategorizedRepository.DataModelRepo.Strategies.imp;

internal class UpsertAggregateStrategy<TAggregateDatabaseModel,
        TLookupDatabaseModel>
    : IUpsertAggregateStrategy<TAggregateDatabaseModel,
        TLookupDatabaseModel>
    where TAggregateDatabaseModel : class, IAggregateDataModel
    where TLookupDatabaseModel : IRepositoryLookup
{
    public UpsertAggregateStrategy(
        ICategoryIndexManipulator<TAggregateDatabaseModel, TLookupDatabaseModel>
            indexManipulator,
        IUnitOfWork<TAggregateDatabaseModel, TLookupDatabaseModel> unitOfWork)
    {
        _indexManipulator = indexManipulator;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public async Task UpsertAsync(TAggregateDatabaseModel aggregate,
        CancellationToken cancellationToken)
    {
        var index =
            await _unitOfWork.GetNonDeletedItemsCategoryIndex(CancellationToken
                .None);

        _indexManipulator.Upsert(index, aggregate);

        await _unitOfWork.UpsertAggregateAsync(aggregate,
            CancellationToken.None);

        await _unitOfWork.UpsertNonDeletedItemsCategoryIndex(index,
            CancellationToken.None);
    }

    private readonly
        ICategoryIndexManipulator<TAggregateDatabaseModel, TLookupDatabaseModel>
        _indexManipulator;

    private readonly IUnitOfWork<TAggregateDatabaseModel, TLookupDatabaseModel>
        _unitOfWork;
}