using Common.Api;

namespace IntegrationTests.Common.Types;

public class LookupDataModel : ILookupDataModel
{
    public string Key { get; set; } = "";
    public bool IsDeleted { get; set; }
    public string DeletedTimeStamp { get; set; } = "";

    public string CustomerName { get; set; } = "";

    public int NumberOfOrders { get; set; }
}