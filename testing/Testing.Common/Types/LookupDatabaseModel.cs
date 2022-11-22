using Common.Api;

namespace Testing.Common.Types;

public class LookupDatabaseModel : IRepositoryKey
{
    public string SomeValue { get; set; } = "";

    /// <inheritdoc />
    public string Key { get; set; }
}