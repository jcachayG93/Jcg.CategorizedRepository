using Common.Api;

namespace Testing.Common.Types
{
    public class AggregateDatabaseModel : IRepositoryKey
    {
        public string SomeValue { get; set; }

        /// <inheritdoc />
        public string Key { get; set; }
    }
}