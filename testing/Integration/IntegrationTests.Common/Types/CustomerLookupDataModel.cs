using Jcg.CategorizedRepository.Api;

namespace IntegrationTests.Common.Types;

public class CustomerLookupDataModel : IRepositoryLookup
{
    public string CustomerName { get; set; } = "";

    public int NumberOfOrders { get; set; }
}