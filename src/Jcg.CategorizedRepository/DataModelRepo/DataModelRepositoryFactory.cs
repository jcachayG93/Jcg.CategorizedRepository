using Jcg.CategorizedRepository.Api;
using Jcg.CategorizedRepository.DataModelRepo.Strategies.imp;
using Jcg.CategorizedRepository.DataModelRepo.Support;
using Jcg.CategorizedRepository.DataModelRepo.Support.IndexManipulator;
using Jcg.CategorizedRepository.UoW;

namespace Jcg.CategorizedRepository.DataModelRepo
{
    internal static class DataModelRepositoryFactory
    {
        internal static
            IDataModelRepository<TAggregateDatabaseModel, TLookupDatabaseModel>
            Create<TAggregateDatabaseModel, TLookupDatabaseModel>
            (IUnitOfWork<TAggregateDatabaseModel, TLookupDatabaseModel> unitOfWork,
                IAggregateToLookupMapper<TAggregateDatabaseModel,
                    TLookupDatabaseModel> aggregateToLookupMapper)
            where TAggregateDatabaseModel : class
        {
            var indexFactory = new CategoryIndexFactory<TLookupDatabaseModel>();

            var initializeCategoryStrategy =
                new InitializeCategoryIndexStrategy<TAggregateDatabaseModel,
                    TLookupDatabaseModel>(
                    indexFactory, unitOfWork);

            var queryStrategy =
                new QueryStrategy<TAggregateDatabaseModel,
                    TLookupDatabaseModel>(unitOfWork);

            var indexManipulator =
                new CategoryIndexManipulator<TAggregateDatabaseModel,
                    TLookupDatabaseModel>(
                    aggregateToLookupMapper);

            var upsertAggregateFactory =
                new UpsertAggregateStrategy<TAggregateDatabaseModel,
                        TLookupDatabaseModel>
                    (indexManipulator, unitOfWork);

            var deleteAndRestoreStrategy =
                new DeleteAndRestoreStrategy<TAggregateDatabaseModel,
                    TLookupDatabaseModel>(
                    indexManipulator, unitOfWork);

            var commitStrategy =
                new CommitStrategy<TAggregateDatabaseModel,
                    TLookupDatabaseModel>(
                    unitOfWork);

            return new DataModelRepository<TAggregateDatabaseModel,
                TLookupDatabaseModel>(
                initializeCategoryStrategy,
                queryStrategy,
                upsertAggregateFactory,
                deleteAndRestoreStrategy,
                commitStrategy);
        }
    }
}