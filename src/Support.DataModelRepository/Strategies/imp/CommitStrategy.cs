using Common.Api;
using Support.UnitOfWork;

namespace Support.DataModelRepository.Strategies;

internal class CommitStrategy<TAggregateDatabaseModel,
    TLookupDatabaseModel> : ICommitStrategy
    where TAggregateDatabaseModel : class, IAggregateDataModel
    where TLookupDatabaseModel : IRepositoryLookup
{
    public CommitStrategy(
        IUnitOfWork<TAggregateDatabaseModel, TLookupDatabaseModel>
            unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public Task CommitChangesAsync(CancellationToken cancellationToken)
    {
        return _unitOfWork.CommitChangesAsync(cancellationToken);
    }

    private readonly
        IUnitOfWork<TAggregateDatabaseModel, TLookupDatabaseModel>
        _unitOfWork;
}