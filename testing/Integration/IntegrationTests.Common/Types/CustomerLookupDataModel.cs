using Jcg.CategorizedRepository.Api;

namespace IntegrationTests.Common.Types;

public class CustomerLookupDataModel : IRepositoryLookup
{
    public string CustomerName { get; set; } = "";

    public int NumberOfOrders { get; set; }


    public string Key { get; set; } = "";
    public bool IsDeleted { get; set; }
    public string DeletedTimeStamp { get; set; } = "";
}