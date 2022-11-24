using FluentAssertions;
using Support.UnitOfWork.Api.Exceptions;

namespace CategorizedRepository.IntegrationTests
{
    public class ExceptionsTests : TestBase
    {
        [Fact]
        public async Task Upsert_CategoryNotInitialized_Throws()
        {
            // ************ ARRANGE ************

            var aggregate = RandomAggregate(out var key);

            var sut = CreateSut();

            // ************ ACT ****************

            Func<Task> fun = async () =>
            {
                await sut.UpsertAsync(key, aggregate,
                    CancellationToken.None);
            };

            // ************ ASSERT *************

            await fun.Should()
                .ThrowAsync<CategoryIndexIsUninitializedException>();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task LookupNonDeleted_CategoryNotInitialized_Throws(
            bool lookupNonDeleted)
        {
            // ************ ARRANGE ************

            var aggregate = RandomAggregate(out var key);

            var sut = CreateSut();

            // ************ ACT ****************

            Func<Task> fun = async () =>
            {
                if (lookupNonDeleted)
                {
                    await sut.LookupNonDeletedAsync(CancellationToken.None);
                }
                else
                {
                    await sut.LookupDeletedAsync(CancellationToken.None);
                }
            };

            // ************ ASSERT *************

            await fun.Should()
                .ThrowAsync<CategoryIndexIsUninitializedException>();
        }


        [Fact]
        public async Task UpsertAfterCommit_Throws()
        {
            // ************ ARRANGE ************

            var aggregate = RandomAggregate(out var key);

            var sut = await CreateSutAndInitializeIndex();

            await sut.CommitChangesAsync(CancellationToken.None);

            // ************ ACT ****************

            Func<Task> fun = async () =>
            {
                await sut.UpsertAsync(key, aggregate,
                    CancellationToken.None);
            };

            // ************ ASSERT *************

            await fun.Should()
                .ThrowAsync<UnitOfWorkWasAlreadyCommittedException>();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task LookupAfterCommit_Throws(bool lookupNonDeleted)
        {
            // ************ ARRANGE ************

            var sut = CreateSut();

            await sut.CommitChangesAsync(CancellationToken.None);

            // ************ ACT ****************

            Func<Task> fun = async () =>
            {
                if (lookupNonDeleted)
                {
                    await sut.LookupNonDeletedAsync(CancellationToken.None);
                }
                else
                {
                    await sut.LookupDeletedAsync(CancellationToken.None);
                }
            };

            // ************ ASSERT *************

            await fun.Should()
                .ThrowAsync<CategoryIndexIsUninitializedException>();
        }


        [Fact]
        public async Task CommitTwiceThrows()
        {
            // ************ ARRANGE ************

            var sut = CreateSut();

            await sut.CommitChangesAsync(CancellationToken.None);

            // ************ ACT ****************

            Func<Task> fun = async () =>
            {
                await sut.CommitChangesAsync(CancellationToken.None);
            };

            // ************ ASSERT *************

            await fun.Should()
                .ThrowAsync<UnitOfWorkWasAlreadyCommittedException>();
        }
    }
}