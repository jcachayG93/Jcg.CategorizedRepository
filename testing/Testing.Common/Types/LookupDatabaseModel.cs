using Common.Api;

namespace Testing.Common.Types;

public class LookupDatabaseModel : IRepositoryLookup
{
    public string SomeValue { get; set; } = "";

    /// <inheritdoc />
    public string Key { get; set; }
}