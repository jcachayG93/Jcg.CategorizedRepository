namespace IntegrationTests.Common.Types;

/// <summary>
/// This is the lookup. Is a very simple, lightweight dto that contains the overview data for an aggregate. In this case,
/// the aggregate identity and how many orders it has. But, it does not contain information on each specific order. To get that,
/// the client needs to query the aggregate. This is just an overview.
/// </summary>
public class Lookup
{
    public Guid CustomerId { get; init; }

    public int NumberOfOrders { get; init; }
}