using Jcg.CategorizedRepository.Api;

namespace Testing.CommonV2.Types
{
    public class AggregateDatabaseModel : IAggregateDataModel
    {
        public string SomeValue { get; set; }

        /// <inheritdoc />
    }
}