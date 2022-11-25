using FluentAssertions;
using IntegrationTests.Common.Helpers;
using IntegrationTests.Common.Verifications;
using Testing.Common.Support.Assertions;

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
                x.PayLoad.CustomerName == y.Name &&
                x.PayLoad.NumberOfOrders == y.Orders.Count() &&
                x.Key == y.Id.ToString() &&
                x.IsDeleted == false);
        }


        [Fact]
        public async Task Delete_MovesLookupToDeleted()
        {
            // ************ ARRANGE ************

            var customer = RandomAggregate(out var key);

            await InitializeCategoryIndexAsync();

            var sut = await CreateSutWithCustomer(customer);

            // ************ ACT ****************

            await sut.DeleteAsync(key, CancellationToken.None);

            // ************ ASSERT *************

            var deleted = await sut.LookupDeletedAsync(CancellationToken.None);

            var result = deleted.First();

            result.Key.Should().Be(customer.Id.ToString());
            result.IsDeleted.Should().BeTrue();
            result.PayLoad.CustomerName.Should().Be(customer.Name);
            result.PayLoad.NumberOfOrders.Should().Be(customer.Orders.Count());


            var nonDeleted =
                await sut.LookupNonDeletedAsync(CancellationToken.None);

            nonDeleted.Should().BeEmpty();
        }


        [Fact]
        public async Task Restore_MovesLookupFromDeletedToNonDeleted()
        {
            // ************ ARRANGE ************

            var customer = RandomAggregate(out var key);

            await InitializeCategoryIndexAsync();

            var sut = await CreateSutWithCustomer(customer);

            await sut.DeleteAsync(key, CancellationToken.None);

            // ************ ACT ****************

            await sut.RestoreAsync(key, CancellationToken.None);

            // ************ ASSERT *************

            var nonDeleted =
                await sut.LookupNonDeletedAsync(CancellationToken.None);

            var result = nonDeleted.First();

            result.Key.Should().Be(customer.Id.ToString());
            result.IsDeleted.Should().BeFalse();
            result.PayLoad.CustomerName.Should().Be(customer.Name);
            result.PayLoad.NumberOfOrders.Should().Be(customer.Orders.Count());


            var deleted = await sut.LookupDeletedAsync(CancellationToken.None);

            deleted.Should().BeEmpty();
        }
    }
}