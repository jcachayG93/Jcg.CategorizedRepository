using IntegrationTests.Common.Types;
using Jcg.CategorizedRepository.Api;

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
            NumberOfOrders = aggregate.Orders.Count()
        };
    }
}