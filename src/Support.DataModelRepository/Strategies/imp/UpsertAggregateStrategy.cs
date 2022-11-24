using Common.InternalContracts;
using Jcg.DataAccessRepositories;
using Support.DataModelRepository.Support.IndexManipulator;

namespace Support.DataModelRepository.Strategies.imp;

internal class UpsertAggregateStrategy<TAggregateDatabaseModel,
        TLookupDatabaseModel>
    : IUpsertAggregateStrategy<TAggregateDatabaseModel,
        TLookupDatabaseModel>
    where TAggregateDatabaseModel : class, IAggregateDataModel
    where TLookupDatabaseModel : ILookupDataModel
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