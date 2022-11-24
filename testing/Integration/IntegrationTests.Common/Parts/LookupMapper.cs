using IntegrationTests.Common.Types;
using Jcg.Repositories.Api;

namespace IntegrationTests.Common.Parts;

public class LookupMapper : ILookupMapper<CustomerLookupDataModel, Lookup>
{
    /// <inheritdoc />
    public Lookup Map(CustomerLookupDataModel databaseModel)
    {
        return new()
        {
            CustomerId = Guid.Parse(databaseModel.Key)!,
            NumberOfOrders = databaseModel.NumberOfOrders,
            Name = databaseModel.CustomerName,
            IsDeleted = databaseModel.IsDeleted
        };
    }
}