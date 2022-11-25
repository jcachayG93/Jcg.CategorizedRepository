using Jcg.CategorizedRepository.Api;
using Jcg.CategorizedRepository.UoW;

namespace Jcg.CategorizedRepository.DataModelRepo.Strategies.imp;

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