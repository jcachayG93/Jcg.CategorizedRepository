using Jcg.Repositories.Api;
using Moq;
using Testing.Common.Support.Extensions;
using Testing.Common.Types;

namespace Testing.Common
{
    public static class RandomStuff
    {
        public static string RandomString()
        {
            return Guid.NewGuid().ToString();
        }

        public static IETagDto<AggregateDatabaseModel> RandomAggregateETagDto()
        {
            return new AggregateETag(RandomString(), new());
        }

        public static IETagDto<CategoryIndex<LookupDatabaseModel>>
            RandomCategoryIndexETagDto()
        {
            return new CategoryIndexETag(RandomString(), new());
        }

        public static AggregateDatabaseModel RandomAggregateDatabaseModel()
        {
            return new()
            {
                SomeValue = RandomString(),
                Key = RandomString()
            };
        }

        public static CategoryIndex<LookupDatabaseModel> RandomCategoryIndex()
        {
            return new();
        }

        public static CategoryIndex<LookupDatabaseModel> CreateCategoryIndex(
            int numberOfLookups)
        {
            var lookups = Enumerable.Range(0, numberOfLookups)
                .Select(i => new LookupDatabaseModel()
                {
                    SomeValue = RandomString()
                }).ToList();

            return new CategoryIndex<LookupDatabaseModel>()
            {
                Lookups = lookups
            };
        }

        public static CategoryIndex<LookupDatabaseModel> CreateCategoryIndex(out LookupDatabaseModel item1,
            out LookupDatabaseModel item2)
        {
            item1 = new();
            item2 = new();

            var lookups = item1.ToCollection(item2);

            return new CategoryIndex<LookupDatabaseModel>()
            {
                Lookups = lookups
            };
        }

        public static CategoryIndex<LookupDatabaseModel> CreateCategoryIndex(
            params string[] keys)
        {
            var lookups = keys.Select(k => new LookupDatabaseModel()
            {
                Key = k
            }).ToList();

            return new()
            {
                Lookups = lookups
            };
        }

        public static AggregateDatabaseModel CreateAggregateDatabaseModel(
            out string value)
        {
            value = RandomString();

            return new()
            {
                SomeValue = value,
                Key = RandomString()
            };
        }

        public static AggregateDatabaseModel CreateAggregateDatabaseModel(
            string key)
        {
            return new()
            {
                Key = key,
                SomeValue = RandomString()
            };
        }


        public static CancellationToken AnyCt()
        {
            return It.IsAny<CancellationToken>();
        }

        public static Guid AnyId()
        {
            return It.IsAny<Guid>();
        }

        public static string AnyString()
        {
            return It.IsAny<string>();
        }
    }
}