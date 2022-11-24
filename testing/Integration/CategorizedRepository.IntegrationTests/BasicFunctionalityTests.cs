using FluentAssertions;
using IntegrationTests.Common.Helpers;
using IntegrationTests.Common.Verifications;
using Testing.Common.Assertions;

namespace CategorizedRepository.IntegrationTests
{
    public class BasicFunctionalityTests : TestBase
    {
        [Fact]
        public async Task GetAggregate_NoMatch_ReturnsNull()
        {
            // ************ ARRANGE ************

            var key = RandomKey();

            var sut = CreateSut();

            // ************ ACT ****************

            var result =
                await sut.GetAggregateAsync(key, CancellationToken.None);

            // ************ ASSERT *************

            result.Should().BeNull();
        }


        [Fact]
        public async Task UpsertThenGet()
        {
            // ************ ARRANGE ************

            var aggregate = RandomAggregate(out var key);

            var sut = await CreateSutAndInitializeIndex();

            await sut.UpsertAsync(key, aggregate, CancellationToken.None);

            // ************ ACT ****************

            var result =
                await sut.GetAggregateAsync(key, CancellationToken.None);

            // ************ ASSERT *************

            result.ShouldBeEquivalentTo(aggregate);
        }


        [Fact]
        public async Task UpsertThenLookup()
        {
            // ************ ARRANGE ************

            RandomAggregates(5, out var aggregates);

            var sut = await CreateSutAndInitializeIndex();

            foreach (var customer in aggregates)
            {
                await sut.UpsertAsync(customer.Id.ToKey(), customer,
                    CancellationToken.None);
            }

            // ************ ACT ****************

            var lookups =
                await sut.LookupNonDeletedAsync(CancellationToken.None);

            // ************ ASSERT *************

            lookups.ShouldBeEquivalent(aggregates, (x, y) =>
                x.Name == y.Name &&
                x.CustomerId == y.Id &&
                x.NumberOfOrders == y.Orders.Count() &&
                x.IsDeleted == false);
        }


        [Fact]
        public async Task Delete_MovesLookupToDeleted()
        {
            // ************ ARRANGE ************

            var customer = RandomAggregate(out var key);

            var sut = await CreateSutWithCustomer(customer);

            // ************ ACT ****************

            await sut.DeleteAsync(key, CancellationToken.None);

            // ************ ASSERT *************

            var deleted = await sut.LookupDeletedAsync(CancellationToken.None);

            var result = deleted.First();

            result.CustomerId.Should().Be(customer.Id);
            result.Name.Should().Be(customer.Name);
            result.NumberOfOrders.Should().Be(customer.Orders.Count());
            result.IsDeleted.Should().BeTrue();

            var nonDeleted =
                await sut.LookupNonDeletedAsync(CancellationToken.None);

            nonDeleted.Should().BeEmpty();
        }


        [Fact]
        public async Task Restore_MovesLookupFromDeletedToNonDeleted()
        {
            // ************ ARRANGE ************

            var customer = RandomAggregate(out var key);

            var sut = await CreateSutWithCustomer(customer);

            await sut.DeleteAsync(key, CancellationToken.None);

            // ************ ACT ****************

            await sut.RestoreAsync(key, CancellationToken.None);

            // ************ ASSERT *************

            var nonDeleted =
                await sut.LookupNonDeletedAsync(CancellationToken.None);

            var result = nonDeleted.First();

            result.CustomerId.Should().Be(customer.Id);
            result.Name.Should().Be(customer.Name);
            result.NumberOfOrders.Should().Be(customer.Orders.Count());
            result.IsDeleted.Should().BeFalse();

            var deleted = await sut.LookupDeletedAsync(CancellationToken.None);

            deleted.Should().BeEmpty();
        }
    }
}