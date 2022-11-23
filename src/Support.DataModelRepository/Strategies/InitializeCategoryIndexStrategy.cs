using Common.Api;
using Support.DataModelRepository.Support;
using Support.UnitOfWork;

namespace Support.DataModelRepository.Strategies
{
    internal interface IInitializeCategoryIndexStrategy
    {
        Task InitializeCategoryIndexes(CancellationToken cancellationToken);
    }

    internal class InitializeCategoryIndexStrategy<TAggregateDatabaseModel,
            TLookupDatabaseModel>
        : IInitializeCategoryIndexStrategy
        where TAggregateDatabaseModel : class, IAggregateDataModel
        where TLookupDatabaseModel : IRepositoryLookup
    {
        public InitializeCategoryIndexStrategy(
            CategoryIndexFactory<TLookupDatabaseModel> indexFactory,
            IUnitOfWork<TAggregateDatabaseModel, TLookupDatabaseModel>
                unitOfWork)
        {
        }

        /// <inheritdoc />
        public Task InitializeCategoryIndexes(
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}