﻿using Jcg.CategorizedRepository.Api;
using Jcg.CategorizedRepository.Api.Exceptions;
using Jcg.CategorizedRepository.DataModelRepo.Support;
using Jcg.CategorizedRepository.UoW;

namespace Jcg.CategorizedRepository.DataModelRepo.Strategies.imp
{
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
            _indexFactory = indexFactory;
            _unitOfWork = unitOfWork;
        }

        /// <inheritdoc />
        public async Task InitializeCategoryIndexes(
            CancellationToken cancellationToken)
        {
            if (await _unitOfWork.CategoryIndexIsInitializedAsync(
                    CancellationToken.None))
            {
                throw new CategoryIndexIsAlreadyInitializedException();
            }

            await _unitOfWork.UpsertDeletedItemsCategoryIndex(
                _indexFactory.Create(), CancellationToken.None);

            await _unitOfWork.UpsertNonDeletedItemsCategoryIndex(
                _indexFactory.Create(), CancellationToken.None);
        }

        private readonly CategoryIndexFactory<TLookupDatabaseModel>
            _indexFactory;

        private readonly
            IUnitOfWork<TAggregateDatabaseModel, TLookupDatabaseModel>
            _unitOfWork;
    }
}