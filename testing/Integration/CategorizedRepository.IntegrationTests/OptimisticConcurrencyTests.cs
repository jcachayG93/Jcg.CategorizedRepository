using FluentAssertions;
using IntegrationTests.Common.Database;

namespace CategorizedRepository.IntegrationTests
{
    public class OptimisticConcurrencyTests : TestBase
    {
        [Fact]
        public async Task UpsertAggregate()
        {
            // ************ ARRANGE ************


            var aggregate = RandomAggregate(out var key);

            await InitializeCategoryIndexAsync();

            await AddAgregateAsync(aggregate);

            var sut1 = CreateSut();
            var sut2 = CreateSut();


            // User 1 gets the aggregate
            aggregate =
                await sut1.GetAggregateAsync(key, CancellationToken.None);

            // User 2 changes the same aggregate
            await sut2.UpsertAsync(key, aggregate, CancellationToken.None);
            await sut2.CommitChangesAsync(CancellationToken.None);

            await sut1.UpsertAsync(key, aggregate, CancellationToken.None);

            // ************ ACT ****************

            Func<Task> fun = async () =>
            {
                // Now, user 1 commits his changes but is too late, user 2 already did. So, this should fail.
                await sut1.CommitChangesAsync(CancellationToken.None);
            };

            // ************ ASSERT *************

            await fun.Should().ThrowAsync<OptimisticConcurrencyException>();
        }
    }
}