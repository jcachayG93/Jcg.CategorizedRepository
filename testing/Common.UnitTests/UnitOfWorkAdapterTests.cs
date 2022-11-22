using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.InternalServices.UnitOfWork;
using Common.UnitTests.TestCommon;
using FluentAssertions;
using Testing.Common.Types;

namespace Common.UnitTests
{
    public class UnitOfWorkAdapterTests
    {
        public UnitOfWorkAdapterTests()
        {
            Adaptee = new();

            Sut = new(Adaptee.Object);

            CancellationToken = new CancellationToken();
        }
        private UnitOfWorkImpMock Adaptee { get; }

        private UnitOfWorkAdapter<AggregateDatabaseModel,LookupDatabaseModel> Sut { get; }

        public CancellationToken CancellationToken { get; }

        [Fact]
        public async Task GetNonDeletedItemsCategoryIndex_DelegatesToAdaptee()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            var result = await
                Sut.GetNonDeletedItemsCategoryIndex(CancellationToken);

            // ************ ASSERT *************

            Adaptee.VerifyGetNonDeletedItemsCategoryIndex(CancellationToken);

            result.Should().BeSameAs(Adaptee.GetNonDeletedItemsCategoryIndexReturns);
        }

        [Fact]
        public async Task GetDeletedItemsCategoryIndex_DelegatesToAdaptee()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            var result = await
                Sut.GetDeletedItemsCategoryIndex(CancellationToken);

            // ************ ASSERT *************

            Adaptee.VerifyGetDeletedItemsCategoryIndex(CancellationToken);

            result.Should().BeSameAs(Adaptee.GetDeletedItemsCategoryIndexReturns);
        }

        [Fact]
        public async Task UpsertDeletedItemsCategoryIndex_DelegatesToAdaptee()
        {
            // ************ ARRANGE ************

            var categoryIndex = RandomCategoryIndex();

            // ************ ACT ****************

            await Sut.UpsertDeletedItemsCategoryIndex(categoryIndex, CancellationToken);

            // ************ ASSERT *************

            Adaptee.VerifyUpsertDeletedItemsCategoryIndex(
                categoryIndex, CancellationToken);
        }

        [Fact]
        public async Task UpsertNonDeletedItemsCategoryIndex_DelegatesToAdaptee()
        {
            // ************ ARRANGE ************

            var categoryIndex = RandomCategoryIndex();

            // ************ ACT ****************

            await Sut.UpsertNonDeletedItemsCategoryIndex(categoryIndex, CancellationToken);

            // ************ ASSERT *************

            Adaptee.VerifyUpsertNonDeletedItemsCategoryIndex(
                categoryIndex, CancellationToken);
        }

        [Fact]
        public async Task GetAggregate_DelegatesToAdaptee()
        {
            // ************ ARRANGE ************

            var key = RandomString();

            // ************ ACT ****************

            var result = await Sut.GetAggregateAsync(key, CancellationToken);

            // ************ ASSERT *************

            Adaptee.VerifyGetAggregate(key, CancellationToken);

            result.Should().BeSameAs(Adaptee.GetAggregateReturns);
        }

        [Fact]
        public async Task UpsertAggregate_DelegatesToAdaptee()
        {
            // ************ ARRANGE ************

            var key = RandomString();

            var aggregate = RandomAggregateDatabaseModel();

            // ************ ACT ****************

            await Sut.UpsertAggregateAsync(key, aggregate, CancellationToken);

            // ************ ASSERT *************

            Adaptee.VerifyUpsertAggregate(key, aggregate, CancellationToken);
        }

        [Fact]
        public async Task CommitChanges_DelegatesToAdaptee()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            await Sut.CommitChangesAsync(CancellationToken);

            // ************ ASSERT *************

            Adaptee.VerifyCommitChanges(CancellationToken);
        }
    }
}
