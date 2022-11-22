using Common.Api;

namespace Testing.Common.Types
{
    public class AggregateDatabaseModel : IAggregateDataModel
    {
        public string SomeValue { get; set; }

        /// <inheritdoc />

        /// <inheritdoc />
        public string Key { get; set; }
    }
}