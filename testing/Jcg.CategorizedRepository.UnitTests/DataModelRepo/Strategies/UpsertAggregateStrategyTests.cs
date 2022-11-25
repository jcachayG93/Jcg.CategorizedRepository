﻿using Jcg.CategorizedRepository.DataModelRepo.Strategies.imp;
using Jcg.CategorizedRepository.UnitTests.DataModelRepo.TestCommon;
using Testing.CommonV2.Mocks;
using Testing.CommonV2.Types;

namespace Jcg.CategorizedRepository.UnitTests.DataModelRepo.Strategies
{
    public class UpsertAggregateStrategyTests
    {
        public UpsertAggregateStrategyTests()
        {
            IndexManipulator = new();
            UnitOfWork = new();
            Sut = new(IndexManipulator.Object, UnitOfWork.Object);

            Aggregate = RandomAggregateDatabaseModel();
        }

        private CategoryIndexManipulatorMock IndexManipulator { get; }

        private UnitOfWorkMock UnitOfWork { get; }

        private UpsertAggregateStrategy<AggregateDatabaseModel,
            Lookup> Sut { get; }


        public AggregateDatabaseModel Aggregate { get; }


        [Fact]
        public async Task Upsert_GetsDeletedItemsCategoryIndex()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            await Sut.UpsertAsync(Aggregate, CancellationToken.None);

            // ************ ASSERT *************

            UnitOfWork.VerifyGetNonDeletedCategoryIndex();
        }


        [Fact]
        public async Task Upsert_UpsertsAggregateToIndexManipulator()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            await Sut.UpsertAsync(Aggregate, CancellationToken.None);

            // ************ ASSERT *************

            IndexManipulator.VerifyUpsert(
                UnitOfWork.GetNonDeletedItemsCategoryIndexReturns, Aggregate);
        }


        [Fact]
        public async Task Upsert_UpsertsAggregateToUnitOfWork()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            await Sut.UpsertAsync(Aggregate, CancellationToken.None);

            // ************ ASSERT *************

            UnitOfWork.VerifyUpsertAggregate(Aggregate);
        }

        [Fact]
        public async Task Upsert_UpsertsNonDeletedCategoryIndexToUnitOfWork()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            await Sut.UpsertAsync(Aggregate, CancellationToken.None);

            // ************ ASSERT *************

            UnitOfWork.VerifyUpsertNonDeletedItemsCategoryIndex(UnitOfWork
                .GetNonDeletedItemsCategoryIndexReturns);
        }
    }
}