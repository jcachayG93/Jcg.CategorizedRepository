using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Api;
using Common.Api.Api;
using Common.InternalContracts;
using Support.DataModelRepository.IndexManipulator;
using Support.DataModelRepository.Strategies;
using Support.DataModelRepository.Strategies.imp;
using Support.DataModelRepository.Support;
using Support.UnitOfWork;

namespace Support.DataModelRepository
{
    internal static class DataModelRepositoryFactory
    {
        internal static IDataModelRepository<TAggregateDatabaseModel, TLookupDatabaseModel> Create<TAggregateDatabaseModel, TLookupDatabaseModel>
            (IUnitOfWork<TAggregateDatabaseModel, TLookupDatabaseModel> unitOfWork,
                IAggregateToLookupMapper<TAggregateDatabaseModel, TLookupDatabaseModel> aggregateToLookupMapper)
            where TAggregateDatabaseModel : class, IAggregateDataModel
            where TLookupDatabaseModel : IRepositoryLookup
        {
            var indexFactory = new CategoryIndexFactory<TLookupDatabaseModel>();

            var initializeCategoryStrategy =
                new InitializeCategoryIndexStrategy<TAggregateDatabaseModel, TLookupDatabaseModel>(
                    indexFactory, unitOfWork);

            var queryStrategy = new QueryStrategy<TAggregateDatabaseModel, TLookupDatabaseModel>(unitOfWork);

            var indexManipulator = new CategoryIndexManipulator<TAggregateDatabaseModel, TLookupDatabaseModel>(
                aggregateToLookupMapper);

            var upsertAggregateFactory = new UpsertAggregateStrategy<TAggregateDatabaseModel, TLookupDatabaseModel>
                (indexManipulator, unitOfWork);

            var deleteAndRestoreStrategy = new DeleteAndRestoreStrategy<TAggregateDatabaseModel, TLookupDatabaseModel>(
                indexManipulator, unitOfWork);

            var commitStrategy = new CommitStrategy<TAggregateDatabaseModel, TLookupDatabaseModel>(
                unitOfWork);

            return new DataModelRepository<TAggregateDatabaseModel, TLookupDatabaseModel>(
                initializeCategoryStrategy,
                queryStrategy,
                upsertAggregateFactory,
                deleteAndRestoreStrategy,
                commitStrategy);
        }
    }
}
