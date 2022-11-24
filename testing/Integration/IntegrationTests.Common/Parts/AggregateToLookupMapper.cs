using Common.Api.Api;
using IntegrationTests.Common.Types;

namespace IntegrationTests.Common.Parts;

public class AggregateToLookupMapper : IAggregateToLookupMapper<
    CustomerDataModel, CustomerLookupDataModel>
{
    /// <inheritdoc />
    public CustomerLookupDataModel ToLookup(CustomerDataModel aggregate)
    {
        return new()
        {
            CustomerName = aggregate.Name,
            NumberOfOrders = aggregate.Orders.Count(),
            Key = aggregate.Key,
            IsDeleted = false,
            DeletedTimeStamp = ""
        };
    }
}