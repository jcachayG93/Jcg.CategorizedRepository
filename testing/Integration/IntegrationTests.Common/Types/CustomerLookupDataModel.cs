using Jcg.Repositories.Api;

namespace IntegrationTests.Common.Types;

public class CustomerLookupDataModel : ILookupDataModel
{
    public string CustomerName { get; set; } = "";

    public int NumberOfOrders { get; set; }


    public string Key { get; set; } = "";
    public bool IsDeleted { get; set; }
    public string DeletedTimeStamp { get; set; } = "";
}