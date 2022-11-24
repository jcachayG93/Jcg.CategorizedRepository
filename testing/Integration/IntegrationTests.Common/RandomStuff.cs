using IntegrationTests.Common.Types;
using Support.UnitOfWork.Api;
using Testing.Common.Extensions;

namespace IntegrationTests.Common
{
    public static class RandomStuff
    {
        public static string RandomKey()
        {
            return Guid.NewGuid().ToString();
        }

        public static string RandomEtag()
        {
            return Guid.NewGuid().ToString();
        }

        public static CustomerDataModel RandomCustomerDataModel(out string key)
        {
            key = RandomKey();

            return new()
            {
                Key = key
            };
        }

        public static CategoryIndex<LookupDataModel> RandomCategoryIndex(
            string oneItemName)
        {
            var lookup = new LookupDataModel()
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