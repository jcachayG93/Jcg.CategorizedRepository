using Common.Api;
using Support.DataModelRepository.IndexManipulator;
using Support.UnitOfWork;

namespace Support.DataModelRepository.Strategies.imp;

internal class DeleteAndRestoreStrategy<TAggregateDatabaseModel,
        TLookupDatabaseModel>
    : IDeleteAndRestoreStrategy<TAggregateDatabaseModel,
        TLookupDatabaseModel>
    where TAggregateDatabaseModel : class, IAggregateDataModel
    where TLookupDatabaseModel : IRepositoryLookup
{
    public DeleteAndRestoreStrategy(
        ICategoryIndexManipulator<TAggregateDatabaseModel,
            TLookupDatabaseModel> indexManipulator,
        IUnitOfWork<TAggregateDatabaseModel, TLookupDatabaseModel>
            unitOfWork)
    {
        _indexManipulator = indexManipulator;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid key, CancellationToken cancellationToken)
    {
        await Execute(key, ActionType.Delete);
    }

    /// <inheritdoc />
    public async Task RestoreAsync(Guid key,
        CancellationToken cancellationToken)
    {
        await Execute(key, ActionType.Restore);
    }

    private async Task Execute(
        Guid key,
        ActionType actionType)
    {
        var nonDeleted =
            await _unitOfWork.GetNonDeletedItemsCategoryIndex(CancellationToken
                .None);

        var deleted =
            await _unitOfWork.GetDeletedItemsCategoryIndex(CancellationToken
                .None);

        if (actionType == ActionType.Delete)
        {
            _indexManipulator.Delete(nonDeleted, deleted, key.ToString(),
                DateTime.Now);
        }
        else
        {
            _indexManipulator.Restore(nonDeleted, deleted, key.ToString());
        }

        await _unitOfWork.UpsertNonDeletedItemsCategoryIndex(nonDeleted,
            CancellationToken.None);

        await _unitOfWork.UpsertDeletedItemsCategoryIndex(deleted,
            CancellationToken.None);
    }


    private readonly
        ICategoryIndexManipulator<TAggregateDatabaseModel, TLookupDatabaseModel>
        _indexManipulator;

    private readonly IUnitOfWork<TAggregateDatabaseModel, TLookupDatabaseModel>
        _unitOfWork;

    private enum ActionType
    {
        Delete,
        Restore
    }
}