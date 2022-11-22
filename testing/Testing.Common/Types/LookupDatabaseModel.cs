using Common.Api;

namespace Testing.Common.Types;

public class LookupDatabaseModel : IRepositoryLookup
{
    public string SomeValue { get; set; }

    /// <inheritdoc />
    public string Key { get; set; }

    /// <inheritdoc />
    public bool IsDeleted { get; set; }

    /// <inheritdoc />
    public string DeletedTimeStamp { get; set; }
}