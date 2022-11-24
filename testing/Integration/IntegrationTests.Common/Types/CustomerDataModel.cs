using Jcg.DataAccessRepositories;

namespace IntegrationTests.Common.Types
{
    /// <summary>
    ///     The Customer data model is the Aggregate data model. Is the same data as the Aggregate but, this time, tailored to
    ///     work well in a database. It has public automatic properties and a parameterless constructor.
    ///     Note also that we have different, independent models for the database and the aggregate. So, the aggregate can
    ///     change more frequently.
    /// </summary>
    public class CustomerDataModel : IAggregateDataModel
    {
        public string Name { get; set; } = "";

        public IEnumerable<OrderDataModel> Orders { get; set; }
            = Array.Empty<OrderDataModel>();

        //The key is just the Id as string
        public string Key { get; set; } = "";


        public class OrderDataModel
        {
            public Guid Id { get; set; }
        }
    }
}