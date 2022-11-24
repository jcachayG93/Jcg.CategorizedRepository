using FluentAssertions;

namespace CategorizedRepository.IntegrationTests
{
    public class UnitOfWorkBehaviourTests : TestBase
    {
        [Fact]
        public async Task ChangesAreLocal_UntilCommit()
        {
            // ************ ARRANGE ************

            await InitializeCategoryIndexAsync();

            var aggregate = RandomAggregate(out var key);

            var sut1 = CreateSut();
            var sut2 = CreateSut();

            await sut1.UpsertAsync(key, aggregate, CancellationToken.None);


            // ************ ACT ****************

            var aggregateFromSut2 =
                await sut2.GetAggregateAsync(key, CancellationToken.None);

            // ************ ASSERT *************

            aggregateFromSut2.Should().BeNull();
        }


        [Fact]
        public async Task EachSutHasItsOwnIndependentData()
        {
            // ************ ARRANGE ************

            await InitializeCategoryIndexAsync();

            var aggregate = RandomAggregate(out var key);

            await AddAgregateAsync(aggregate);

            var sut1 = CreateSut();
            var sut2 = CreateSut();

            var a1 = await sut1.GetAggregateAsync(key, CancellationToken.None);
            var a2 = await sut2.GetAggregateAsync(key, CancellationToken.None);

            a1.UpdateName("AAA");
            a2.UpdateName("BBB");

            await sut1.UpsertAsync(key, a1, CancellationToken.None);
            await sut2.UpsertAsync(key, a2, CancellationToken.None);

            // ************ ACT ****************

            var result1 =
                await sut1.GetAggregateAsync(key, CancellationToken.None);

            var result2 =
                await sut2.GetAggregateAsync(key, CancellationToken.None);

            // ************ ASSERT *************

            result1.Name.Should().Be("AAA");

            result2.Name.Should().Be("BBB");
        }
    }
}