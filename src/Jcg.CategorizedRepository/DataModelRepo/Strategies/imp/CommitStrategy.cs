using Common.InternalContracts;
using Jcg.DataAccessRepositories;

namespace Support.DataModelRepository.Strategies.imp;

internal class CommitStrategy<TAggregateDatabaseModel,
    TLookupDatabaseModel> : ICommitStrategy
    where TAggregateDatabaseModel : class, IAggregateDataModel
    where TLookupDatabaseModel : ILookupDataModel
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