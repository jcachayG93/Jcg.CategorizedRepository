using FluentAssertions;
using Support.UnitOfWork.Api.Exceptions;
using Testing.Common.Assertions;
using Testing.Common.Doubles;

namespace Support.UnitOfWork.IntegrationTests
{
    public class UnitOfWorkIsolationTests : TestBase
    {
        private async Task AssertThrows<TException>(Func<Task> f)
            where TException : Exception
        {
            await f.Should().ThrowAsync<TException>();
        }

        private async Task AssertDoesntThrows(Func<Task> f)

        {
            await f.Should().NotThrowAsync();
        }


        /*
         * Changes exist in the Unit of work but not in the database until committed.
         * The following tests will create two UnitOfWork instances (sut1, sut2), make changes in one. assert that data exist in the other one only after data is committed
         */

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public async Task DeletedCategoryIndex_NoCommit_KeepsChangesLocal(
            bool callCommit, bool dataShouldExistInSut2)
        {
            // ************ ARRANGE ************

            var categoryIndex = CreateCategoryIndex(3);

            var sut1 = CreateSut();

            var sut2 = CreateSut();

            // ************ ACT ****************

            await sut1.UpsertDeletedItemsCategoryIndex(categoryIndex,
                CancellationToken.None);

            if (callCommit)
            {
                await sut1.CommitChangesAsync(CancellationToken.None);
            }

            // ************ ASSERT *************

            if (dataShouldExistInSut2)
            {
                var result =
                    await sut2.GetDeletedItemsCategoryIndex(CancellationToken
                        .None);

                result.Lookups.ShouldBeEquivalent(categoryIndex.Lookups,
                    (x, y) => x.SomeValue == y.SomeValue);
            }
            else
            {
                await AssertThrows<CategoryIndexIsUninitializedException>(
                    () => sut2.GetDeletedItemsCategoryIndex(
                        CancellationToken
                            .None));
            }
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public async Task NonDeletedCategoryIndex_NoCommit_KeepsChangesLocal(
            bool callCommit, bool dataShouldExistInSut2)
        {
            // ************ ARRANGE ************

            var categoryIndex = CreateCategoryIndex(3);

            var sut1 = CreateSut();

            var sut2 = CreateSut();

            // ************ ACT ****************

            await sut1.UpsertNonDeletedItemsCategoryIndex(categoryIndex,
                CancellationToken.None);

            if (callCommit)
            {
                await sut1.CommitChangesAsync(CancellationToken.None);
            }

            // ************ ASSERT *************

            if (dataShouldExistInSut2)
            {
                var result =
                    await sut2.GetNonDeletedItemsCategoryIndex(CancellationToken
                        .None);

                result.Lookups.ShouldBeEquivalent(categoryIndex.Lookups,
                    (x, y) => x.SomeValue == y.SomeValue);
            }
            else
            {
                await AssertThrows<CategoryIndexIsUninitializedException>(
                    () => sut2.GetNonDeletedItemsCategoryIndex(
                        CancellationToken
                            .None));
            }
        }


        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public async Task UpsertAggregate_NoCommit_KeepsChangesLocal(
            bool callCommit, bool dataShouldExistInSut2)
        {
            // ************ ARRANGE ************

            var aggregate = CreateAggregateDatabaseModel(out var value);

            var key = RandomString();

            var sut1 = CreateSut();

            var sut2 = CreateSut();


            // ************ ACT ****************

            await sut1.UpsertAggregateAsync(key, aggregate,
                CancellationToken.None);

            if (callCommit)
            {
                await sut1.CommitChangesAsync(CancellationToken.None);
            }

            // ************ ASSERT *************

            if (dataShouldExistInSut2)
            {
                var result =
                    await sut2.GetAggregateAsync(key, CancellationToken.None);

                result!.SomeValue.Should().Be(value);
            }
            else
            {
                (await sut2.GetAggregateAsync(key, CancellationToken.None))
                    .Should().BeNull();
            }
        }

        /*
         * Optimistic concurrency. The following test check that whichever commits first wins.
         * Create two suts, upsert the data for the same key on both. Then commit as the parameters of the test, assert.
         * The UnitOfWork (SUT) does not make the concurrency check, all it does is pass the received ETag to the database, the database
         * is responsible for making this check. For this particular database (InMemoryDataSource.cs) that logic is implemented.
         * With this test, I am asserting that the UnitOfWork is in fact sending the correct ETags
         */


        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public async Task DeletedCategoryIndex_OptimisticConcurrencyCheck(
            bool sut1WasCommittedBeforeSut2, bool sut2ShouldThrow)
        {
            // ************ ARRANGE ************


            var sut1 = CreateSut();

            var sut2 = CreateSut();

            // ************ ACT ****************

            await sut1.UpsertDeletedItemsCategoryIndex(RandomCategoryIndex(),
                CancellationToken.None);

            await sut2.UpsertDeletedItemsCategoryIndex(RandomCategoryIndex(),
                CancellationToken.None);

            if (sut1WasCommittedBeforeSut2)
            {
                await sut1.CommitChangesAsync(CancellationToken.None);
            }


            // ************ ASSERT *************

            if (sut2ShouldThrow)
            {
                await AssertThrows<DatabaseException>(() =>
                    sut2.CommitChangesAsync(CancellationToken.None));
            }
            else
            {
                await AssertDoesntThrows(() =>
                    sut2.CommitChangesAsync(CancellationToken.None));
            }
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public async Task NonDeletedCategoryIndex_OptimisticConcurrencyCheck(
            bool sut1WasCommittedBeforeSut2, bool sut2ShouldThrow)
        {
            // ************ ARRANGE ************


            var sut1 = CreateSut();

            var sut2 = CreateSut();

            // ************ ACT ****************

            await sut1.UpsertNonDeletedItemsCategoryIndex(RandomCategoryIndex(),
                CancellationToken.None);

            await sut2.UpsertNonDeletedItemsCategoryIndex(RandomCategoryIndex(),
                CancellationToken.None);

            if (sut1WasCommittedBeforeSut2)
            {
                await sut1.CommitChangesAsync(CancellationToken.None);
            }


            // ************ ASSERT *************

            if (sut2ShouldThrow)
            {
                await AssertThrows<DatabaseException>(() =>
                    sut2.CommitChangesAsync(CancellationToken.None));
            }
            else
            {
                await AssertDoesntThrows(() =>
                    sut2.CommitChangesAsync(CancellationToken.None));
            }
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public async Task UpsertAggregates_OptimisticConcurrencyCheck(
            bool sut1WasCommittedBeforeSut2, bool sut2ShouldThrow)
        {
            // ************ ARRANGE ************

            var key = RandomString();

            var sut1 = CreateSut();

            var sut2 = CreateSut();

            // ************ ACT ****************

            await sut1.UpsertAggregateAsync(key, RandomAggregateDatabaseModel(),
                CancellationToken.None);

            await sut2.UpsertAggregateAsync(key, RandomAggregateDatabaseModel(),
                CancellationToken.None);

            if (sut1WasCommittedBeforeSut2)
            {
                await sut1.CommitChangesAsync(CancellationToken.None);
            }


            // ************ ASSERT *************

            if (sut2ShouldThrow)
            {
                await AssertThrows<DatabaseException>(() =>
                    sut2.CommitChangesAsync(CancellationToken.None));
            }
            else
            {
                await AssertDoesntThrows(() =>
                    sut2.CommitChangesAsync(CancellationToken.None));
            }
        }
    }
}