using Common.Api;

namespace IntegrationTests.Common.Types;

public class LookupDataModel : ILookupDataModel, IClone
{
    public string CustomerName { get; set; } = "";

    public int NumberOfOrders { get; set; }

    /// <inheritdoc />
    public object Clone()
    {
        return new()
        {
            Key = Key,
            IsDeleted = IsDeleted,
            DeletedTimeStamp = DeletedTimeStamp,
            CustomerName = CustomerName,
            NumberOfOrders = NumberOfOrders
        };
    }

    public string Key { get; set; } = "";
    public bool IsDeleted { get; set; }
    public string DeletedTimeStamp { get; set; } = "";
}