using Support.CategorizedRepository;
using Support.CategorizedRepository.Support;
using Support.DataModelRepository;
using Support.UnitOfWork;

namespace Jcg.Repositories.Api
{
    public static class CategorizedRepositoryFactory
    {
        public static ICategorizedRepository<TAggregate, TLookup> Create<
            TAggregate, TAggregateDatabaseModel, TLookup, TLookupDatabaseModel>(
            RepositoryIdentity categoryKey,
            ITransactionalDatabaseClient<TAggregateDatabaseModel,
                TLookupDatabaseModel> databaseClient,
            IAggregateMapper<TAggregate, TAggregateDatabaseModel>
                aggregateMapper,
            IAggregateToLookupMapper<TAggregateDatabaseModel,
                TLookupDatabaseModel> aggregateToLookupMapper,
            ILookupMapper<TLookupDatabaseModel, TLookup> lookupMapper)
            where TAggregateDatabaseModel : class, IAggregateDataModel
            where TLookupDatabaseModel : ILookupDataModel
        {
            var unitOfWork = UnitOfWorkFactory.Create(
                categoryKey.Value.ToString(),
                categoryKey.ToDeletedCategoryIndexKey(),
                databaseClient);

            var dataModelRepo = DataModelRepositoryFactory
                .Create(unitOfWork, aggregateToLookupMapper);

            var lookupMapperAdapter =
                new LookupMapperAdapter<TLookupDatabaseModel, TLookup>(
                    lookupMapper);

            return
                new CategorizedRepository<TAggregate, TAggregateDatabaseModel,
                    TLookup, TLookupDatabaseModel>(
                    aggregateMapper,
                    lookupMapperAdapter,
                    dataModelRepo
                );
        }

        private static string ToDeletedCategoryIndexKey(
            this RepositoryIdentity categoryKey)
        {
            return $"{categoryKey.Value}-D";
        }
    }
}