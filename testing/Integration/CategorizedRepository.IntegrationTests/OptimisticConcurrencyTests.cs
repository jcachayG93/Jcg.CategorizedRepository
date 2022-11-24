using FluentAssertions;
using IntegrationTests.Common.Database;
using IntegrationTests.Common.Types;

namespace CategorizedRepository.IntegrationTests
{
    public class OptimisticConcurrencyTests : TestBase
    {
        [Fact]
        public async Task UpsertAggregate()
        {
            // ************ ARRANGE ************


            var aggregate = RandomAggregate(out var key);

            await CreateSutAddAggregateAndCommit(aggregate);

            var sut1 = await CreateSutAndInitializeIndex();

            var sut2 = await CreateSutAndInitializeIndex();


            await sut1.UpsertAsync(key, aggregate, CancellationToken.None);

            await sut2.GetAggregateAsync(key, CancellationToken.None);

            await sut1.CommitChangesAsync(CancellationToken.None);

            // ************ ACT ****************

            Func<Task> fun = async () =>
            {
                await sut2.UpsertAsync(key, aggregate, CancellationToken.None);
                await sut2.CommitChangesAsync(CancellationToken.None);
            };

            // ************ ASSERT *************

            await fun.Should().ThrowAsync<OptimisticConcurrencyException>();
        }

        private async Task CreateSutAddAggregateAndCommit(Customer aggregate)
        {
            var sut = await CreateSutWithCustomer(aggregate);

            await sut.CommitChangesAsync(CancellationToken.None);
        }
    }
}