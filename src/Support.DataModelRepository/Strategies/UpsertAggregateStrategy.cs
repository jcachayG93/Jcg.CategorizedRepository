using Common.Api;
using Support.DataModelRepository.IndexManipulator;
using Support.UnitOfWork;

namespace Support.DataModelRepository.Strategies;

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
    }

    /// <inheritdoc />
    public Task UpsertAsync(TAggregateDatabaseModel aggregate,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}