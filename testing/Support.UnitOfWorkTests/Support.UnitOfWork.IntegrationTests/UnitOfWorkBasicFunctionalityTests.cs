using Common.Api.Exceptions;
using FluentAssertions;
using Support.UnitOfWork.Api.Exceptions;
using Testing.Common.Assertions;
using Testing.Common.Types;

namespace Support.UnitOfWork.IntegrationTests
{
    public class UnitOfWorkBasicFunctionalityTests : TestBase
    {
        public UnitOfWorkBasicFunctionalityTests()
        {
            Sut = CreateSut();
        }

        private IUnitOfWork<AggregateDatabaseModel, LookupDatabaseModel> Sut
        {
            get;
        }

        [Fact]
        public async Task
            GetNonDeletedItemsCategoryIndex_NoCategoryIndex_Throws()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            var fun = new Func<Task>(async () =>
            {
                await Sut.GetNonDeletedItemsCategoryIndex(CancellationToken
                    .None);
            });

            // ************ ASSERT *************

            await fun.Should()
                .ThrowAsync<CategoryIndexIsUninitializedException>();
        }

        [Fact]
        public async Task GetDeletedItemsCategoryIndex_NoCategoryIndex_Throws()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            var fun = new Func<Task>(async () =>
            {
                await Sut.GetDeletedItemsCategoryIndex(CancellationToken
                    .None);
            });

            // ************ ASSERT *************

            await fun.Should()
                .ThrowAsync<CategoryIndexIsUninitializedException>();
        }

        [Fact]
        public async Task DeletedCategoryIndex_Upsert_Then_Get()
        {
            // ************ ARRANGE ************

            var index = CreateCategoryIndex(3);

            // ************ ACT ****************

            await Sut.UpsertDeletedItemsCategoryIndex(index,
                CancellationToken.None);


            var result =
                await Sut.GetDeletedItemsCategoryIndex(CancellationToken.None);

            // ************ ASSERT *************

            result.Lookups.ShouldBeEquivalent(index.Lookups,
                (x, y) => x.SomeValue == y.SomeValue);
        }

        [Fact]
        public async Task NonDeletedCategoryIndex_Upsert_Then_Get()
        {
            // ************ ARRANGE ************

            var index = CreateCategoryIndex(3);

            // ************ ACT ****************

            await Sut.UpsertNonDeletedItemsCategoryIndex(index,
                CancellationToken.None);


            var result =
                await Sut.GetNonDeletedItemsCategoryIndex(
                    CancellationToken.None);

            // ************ ASSERT *************

            result.Lookups.ShouldBeEquivalent(index.Lookups,
                (x, y) => x.SomeValue == y.SomeValue);
        }

        [Fact]
        public async Task GetAggregate_NoAggregate_ReturnsNull()
        {
            // ************ ARRANGE ************


            // ************ ACT ****************

            var result =
                await Sut.GetAggregateAsync(RandomString(),
                    CancellationToken.None);

            // ************ ASSERT *************

            result.Should().BeNull();
        }

        [Fact]
        public async Task UpsertAggregate_Then_Get()
        {
            // ************ ARRANGE ************

            var aggregate = CreateAggregateDatabaseModel(out var value);


            // ************ ACT ****************

            await Sut.UpsertAggregateAsync(aggregate,
                CancellationToken.None);

            var result =
                await Sut.GetAggregateAsync(aggregate.Key,
                    CancellationToken.None);

            // ************ ASSERT *************

            result.SomeValue.Should().Be(value);
        }

        [Fact]
        public async Task Commit_MoreThanOnce_Throws()
        {
            // ************ ARRANGE ************

            await Sut.UpsertAggregateAsync(
                RandomAggregateDatabaseModel(), CancellationToken.None);

            // ************ ACT ****************

            await Sut.CommitChangesAsync(CancellationToken.None);

            var fun = new Func<Task>(async () =>
            {
                await Sut.CommitChangesAsync(CancellationToken.None);
            });

            // ************ ASSERT *************

            await fun.Should()
                .ThrowAsync<UnitOfWorkWasAlreadyCommittedException>();
        }

        [Fact]
        public async Task DeletedCategoryIndex_Upsert_After_Commit_Throws()
        {
            // ************ ARRANGE ************

            var index = RandomCategoryIndex();

            await Sut.UpsertDeletedItemsCategoryIndex(index,
                CancellationToken.None);

            await Sut.CommitChangesAsync(CancellationToken.None);

            var fun = new Func<Task>(async () =>
            {
                await Sut.UpsertDeletedItemsCategoryIndex(index,
                    CancellationToken.None);
            });

            // ************ ASSERT *************

            await fun.Should()
                .ThrowAsync<UnitOfWorkWasAlreadyCommittedException>();
        }

        [Fact]
        public async Task NonDeletedCategoryIndex_Upsert_After_Commit_Throws()
        {
            // ************ ARRANGE ************

            var index = RandomCategoryIndex();

            await Sut.UpsertNonDeletedItemsCategoryIndex(index,
                CancellationToken.None);

            await Sut.CommitChangesAsync(CancellationToken.None);

            var fun = new Func<Task>(async () =>
            {
                await Sut.UpsertNonDeletedItemsCategoryIndex(index,
                    CancellationToken.None);
            });

            // ************ ASSERT *************

            await fun.Should()
                .ThrowAsync<UnitOfWorkWasAlreadyCommittedException>();
        }

        [Fact]
        public async Task UpsertAggregate_After_Commit_Throws()
        {
            // ************ ARRANGE ************

            var aggregate = RandomAggregateDatabaseModel();

            await Sut.UpsertAggregateAsync(aggregate,
                CancellationToken.None);

            await Sut.CommitChangesAsync(CancellationToken.None);


            // ************ ACT ****************

            var fun = new Func<Task>(async () =>
            {
                await Sut.UpsertAggregateAsync(aggregate,
                    CancellationToken.None);
            });

            // ************ ASSERT *************

            await fun.Should()
                .ThrowAsync<UnitOfWorkWasAlreadyCommittedException>();
        }

        [Fact]
        public async Task DeletedCategoryIndex_GetAfterCommit_WorksAsUsual()
        {
            // ************ ARRANGE ************

            var categoryIndex = CreateCategoryIndex(1);

            await Sut.UpsertDeletedItemsCategoryIndex(categoryIndex,
                CancellationToken.None);

            await Sut.CommitChangesAsync(CancellationToken.None);

            // ************ ACT ****************

            var result =
                await Sut.GetDeletedItemsCategoryIndex(CancellationToken.None);

            // ************ ASSERT *************

            result.Lookups.ShouldBeEquivalent(categoryIndex.Lookups,
                (x, y) => x.SomeValue == y.SomeValue);
        }

        [Fact]
        public async Task NonDeletedCategoryIndex_GetAfterCommit_WorksAsUsual()
        {
            // ************ ARRANGE ************

            var categoryIndex = CreateCategoryIndex(1);

            await Sut.UpsertNonDeletedItemsCategoryIndex(categoryIndex,
                CancellationToken.None);

            await Sut.CommitChangesAsync(CancellationToken.None);

            // ************ ACT ****************

            var result =
                await Sut.GetNonDeletedItemsCategoryIndex(
                    CancellationToken.None);

            // ************ ASSERT *************

            result.Lookups.ShouldBeEquivalent(categoryIndex.Lookups,
                (x, y) => x.SomeValue == y.SomeValue);
        }

        [Fact]
        public async Task GetAggregate_AfterCommit_WorksAsUsual()
        {
            // ************ ARRANGE ************

            var aggregate = CreateAggregateDatabaseModel(out var value);


            await Sut.UpsertAggregateAsync(aggregate,
                CancellationToken.None);

            await Sut.CommitChangesAsync(CancellationToken.None);

            // ************ ACT ****************

            var result =
                await Sut.GetAggregateAsync(aggregate.Key,
                    CancellationToken.None);

            // ************ ASSERT *************

            result.SomeValue.Should().Be(value);
        }


        [Theory]
        [InlineData(true, true, true)]
        [InlineData(false, false, false)]
        public async Task
            CategoryIndexIsInitialized_True_IfDeletedAndNonDeletedCategoryIndexExists(
                bool deletedIndexExists, bool nonDeletedIndexExists,
                bool expectedResult)
        {
            // ************ ARRANGE ************

            if (deletedIndexExists)
            {
                await Sut.UpsertDeletedItemsCategoryIndex(RandomCategoryIndex(),
                    CancellationToken.None);
            }

            if (nonDeletedIndexExists)
            {
                await Sut.UpsertNonDeletedItemsCategoryIndex(
                    RandomCategoryIndex(), CancellationToken.None);
            }

            await Sut.CommitChangesAsync(CancellationToken.None);

            // ************ ACT ****************

            var result =
                await Sut.CategoryIndexIsInitializedAsync(
                    CancellationToken.None);

            // ************ ASSERT *************

            result.Should().Be(expectedResult);
        }


        [Theory]
        [InlineData(true, true, false)]
        [InlineData(false, false, false)]
        [InlineData(true, false, true)]
        [InlineData(false, true, true)]
        public async Task OneCategoryIndexExistButTheOtherDont_Throws(
            bool deletedIndexExists, bool nonDeletedIndexExists,
            bool shouldThrow)
        {
            // ************ ARRANGE ************

            if (deletedIndexExists)
            {
                await Sut.UpsertDeletedItemsCategoryIndex(RandomCategoryIndex(),
                    CancellationToken.None);
            }

            if (nonDeletedIndexExists)
            {
                await Sut.UpsertNonDeletedItemsCategoryIndex(
                    RandomCategoryIndex(), CancellationToken.None);
            }

            await Sut.CommitChangesAsync(CancellationToken.None);


            // ************ ACT ****************

            Func<Task> fun = async () =>
            {
                await Sut.CategoryIndexIsInitializedAsync(
                    CancellationToken.None);
            };

            // ************ ASSERT *************

            if (shouldThrow)
            {
                await fun.Should()
                    .ThrowAsync<InternalRepositoryErrorException>();
            }
            else
            {
                await fun.Should().NotThrowAsync();
            }
        }
    }
}