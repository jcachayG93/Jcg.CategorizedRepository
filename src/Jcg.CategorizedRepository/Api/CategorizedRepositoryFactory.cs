﻿using Jcg.CategorizedRepository.CategorizedRepo;
using Jcg.CategorizedRepository.CategorizedRepo.Support;
using Jcg.CategorizedRepository.DataModelRepo;
using Jcg.CategorizedRepository.UoW;

namespace Jcg.CategorizedRepository.Api
{
    public static class CategorizedRepositoryFactory
    {
        public static ICategorizedRepository<TAggregate, TLookup> Create<
            TAggregate, TAggregateDatabaseModel, TLookup>(
            RepositoryIdentity categoryKey,
            ITransactionalDatabaseClient<TAggregateDatabaseModel,
                TLookup> databaseClient,
            IAggregateMapper<TAggregate, TAggregateDatabaseModel>
                aggregateMapper,
            IAggregateToLookupMapper<TAggregateDatabaseModel,
                TLookup> aggregateToLookupMapper)
            where TAggregateDatabaseModel : class, IAggregateDataModel

        where TLookup : IRepositoryLookup
        {
            var unitOfWork = UnitOfWorkFactory.Create(
                categoryKey.Value.ToString(),
                categoryKey.ToDeletedCategoryIndexKey(),
                databaseClient);

            var dataModelRepo = DataModelRepositoryFactory
                .Create(unitOfWork, aggregateToLookupMapper);

            return
                new CategorizedRepository<TAggregate, TAggregateDatabaseModel,
                    TLookup, TLookup>(
                    aggregateMapper,
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