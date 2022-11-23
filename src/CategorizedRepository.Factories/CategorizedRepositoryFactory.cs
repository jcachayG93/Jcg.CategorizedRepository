using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Api;
using Common.Api.Api;
using Support.CategorizedRepository;
using Support.CategorizedRepository.Support;
using Support.DataModelRepository;
using Support.UnitOfWork;
using Support.UnitOfWork.Api;

namespace CategorizedRepository.Factories
{
    public static class CategorizedRepositoryFactory
    {
        public static ICategorizedRepository<TAggregate, TLookup> Create<TAggregate, TAggregateDatabaseModel, TLookup, TLookupDatabaseModel>(
            RepositoryIdentity categoryKey,
            ITransactionalDatabaseClient<TAggregateDatabaseModel, TLookupDatabaseModel> databaseClient,
            IAggregateMapper<TAggregate, TAggregateDatabaseModel> aggregateMapper,
            IAggregateToLookupMapper<TAggregateDatabaseModel, TLookupDatabaseModel> aggregateToLookupMapper,
            ILookupMapper<TLookupDatabaseModel, TLookup> lookupMapper)
            where TAggregateDatabaseModel : class, IAggregateDataModel
            where TLookupDatabaseModel : ILookupDataModel
        {
            var unitOfWork = UnitOfWorkFactory.Create(categoryKey.Value.ToString(),
                categoryKey.ToDeletedCategoryIndexKey(),
                databaseClient);

            var dataModelRepo = DataModelRepositoryFactory
                .Create(unitOfWork, aggregateToLookupMapper);

            var lookupMapperAdapter = new LookupMapperAdapter<TLookupDatabaseModel,TLookup>(lookupMapper);

            return
                new CategorizedRepository<TAggregate, TAggregateDatabaseModel, TLookup, TLookupDatabaseModel>(
                    aggregateMapper,
                    aggregateToLookupMapper,
                    lookupMapperAdapter,
                    dataModelRepo
                );
        }

        private static string ToDeletedCategoryIndexKey(this RepositoryIdentity categoryKey)
        {
            return $"{categoryKey.Value.ToString()}-D";
        }
    }

    
}
