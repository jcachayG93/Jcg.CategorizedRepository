using IntegrationTests.Common.Types;
using Jcg.Repositories.Api;
using Testing.Common.Support.Extensions;

namespace IntegrationTests.Common
{
    public static class RandomStuff
    {
        public static string RandomString()
        {
            return Guid.NewGuid().ToString();
        }

        public static RepositoryIdentity RandomKey()
        {
            return new(Guid.NewGuid());
        }

        public static Customer RandomAggregate(out RepositoryIdentity key)
        {
            key = RandomKey();

            var result = new Customer(key.Value, "Juan Perez");

            for (var i = 0; i < 10; i++)
            {
                result.AddOrder(Guid.NewGuid());
            }

            return result;
        }

        public static void RandomAggregates(
            int count,
            out IEnumerable<Customer> aggregates)
        {
            aggregates = Enumerable.Range(0, count)
                .Select(i => RandomAggregate(out _))
                .ToList();
        }


        public static string RandomEtag()
        {
            return Guid.NewGuid().ToString();
        }

        public static CustomerDataModel RandomCustomerDataModel(out string key)
        {
            key = RandomString();

            return new()
            {
                Key = key
            };
        }

        public static CategoryIndex<CustomerLookupDataModel>
            RandomCategoryIndex(
                string oneItemName)
        {
            var lookup = new CustomerLookupDataModel()
            {
                CustomerName = oneItemName
            };

            return new()
            {
                Lookups = lookup.ToCollection()
            };
        }
    }
}