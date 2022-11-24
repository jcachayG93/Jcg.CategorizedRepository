using FluentAssertions;
using IntegrationTests.Common.Types;
using Testing.Common.Assertions;

namespace IntegrationTests.Common.Verifications
{
    public static class AssertEquivalent
    {
        public static void ShouldBeEquivalentTo(
            this Customer aggregate, Customer other)
        {
            var result = aggregate.Id == other.Id &&
                         aggregate.Name == other.Name;

            result.Should().BeTrue("Name and Id should match");

            aggregate.Orders.ShouldBeEquivalent(other.Orders, (x, y) =>
                x.Id == y.Id);
        }
    }
}