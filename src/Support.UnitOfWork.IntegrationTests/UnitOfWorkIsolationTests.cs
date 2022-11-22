using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Testing.Common.Assertions;

namespace Support.UnitOfWork.IntegrationTests
{
    public class UnitOfWorkIsolationTests : TestBase
    {
        /*
         * Changes exist in the Unit of work but not in the database until committed
         */

        [Fact]
        public async Task DeletedCategoryIndex_NoCommit_KeepsChangesLocal()
        {
            // ************ ARRANGE ************

            var categoryIndex = RandomCategoryIndex();

            // ************ ACT ****************

            await Sut.UpsertDeletedItemsCategoryIndex(categoryIndex, CancellationToken.None);

            var result = await Sut.GetDeletedItemsCategoryIndex(CancellationToken.None);

            // ************ ASSERT *************

            result.Should().NotBeNull();
            
            DataSource.GetCategoryIndex(DeletedCategoryIndexKey).Should().BeNull();
        }

        [Fact]
        public async Task DeleteCategoryIndex_Commit_ApplyChangesInDatabase()
        {
            // ************ ARRANGE ************

            var categoryIndex = CreateCategoryIndex(3);

            // ************ ACT ****************

            await Sut.UpsertDeletedItemsCategoryIndex(categoryIndex, CancellationToken.None);

            await Sut.CommitChangesAsync(CancellationToken.None);

            // ************ ASSERT *************

            var resultFromDatabase = DataSource.GetCategoryIndex(DeletedCategoryIndexKey);

            resultFromDatabase.Payload.Lookups.ShouldBeEquivalent(categoryIndex.Lookups,(x,y)=>x.SomeValue == y.SomeValue);
        }

        [Fact]
        public async Task NonDeletedCategoryIndex_NoCommit_KeepsChangesLocal()
        {
            // ************ ARRANGE ************

            var categoryIndex = RandomCategoryIndex();

            // ************ ACT ****************

            await Sut.UpsertNonDeletedItemsCategoryIndex(categoryIndex, CancellationToken.None);

            var result = await Sut.GetNonDeletedItemsCategoryIndex(CancellationToken.None);

            // ************ ASSERT *************

            result.Should().NotBeNull();

            DataSource.GetCategoryIndex(NonDeletedCategoryIndexKey).Should().BeNull();
        }

        [Fact]
        public async Task NonDeleteCategoryIndex_Commit_ApplyChangesInDatabase()
        {
            // ************ ARRANGE ************

            var categoryIndex = CreateCategoryIndex(3);

            // ************ ACT ****************

            await Sut.UpsertNonDeletedItemsCategoryIndex(categoryIndex, CancellationToken.None);

            await Sut.CommitChangesAsync(CancellationToken.None);

            // ************ ASSERT *************

            var resultFromDatabase = DataSource.GetCategoryIndex(NonDeletedCategoryIndexKey);

            resultFromDatabase.Payload.Lookups.ShouldBeEquivalent(categoryIndex.Lookups, (x, y) => x.SomeValue == y.SomeValue);
        }

        [Fact]
        public async Task UpsertAggregate_NoCommit_KeepsChangesLocal()
        {
            // ************ ARRANGE ************

            var aggregate = RandomAggregateDatabaseModel();

            var key = RandomString();

            // ************ ACT ****************

            await Sut.UpsertAggregateAsync(key, aggregate, CancellationToken.None);

            var result = await Sut.GetAggregateAsync(key, CancellationToken.None);

            // ************ ASSERT *************

            result.Should().NotBeNull();

            DataSource.GetAggregate(key).Should().BeNull();
        }

        [Fact]
        public async Task UpsertAggregate_Commit_ApplyChangesToDatabase()
        {
            // ************ ARRANGE ************

            var aggregate = CreateAggregateDatabaseModel(out var value);

            var key = RandomString();

            // ************ ACT ****************

            await Sut.UpsertAggregateAsync(key, aggregate, CancellationToken.None);

            await Sut.CommitChangesAsync(CancellationToken.None);

            // ************ ASSERT *************

            var result = DataSource.GetAggregate(key);

            var aggregateFromDatabase = DataSource.GetAggregate(key);

            aggregateFromDatabase.Payload.SomeValue.Should().Be(value);
        }
    }
}
