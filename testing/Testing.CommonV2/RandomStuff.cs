using Jcg.CategorizedRepository.Api;
using Moq;
using Testing.Common.Support.Extensions;
using Testing.CommonV2.Types;

namespace Testing.CommonV2
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

        public static IETagDto<CategoryIndex<Lookup>>
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

        public static CategoryIndex<Lookup> RandomCategoryIndex()
        {
            return new();
        }

        public static CategoryIndex<Lookup> CreateCategoryIndex(
            int numberOfLookups)
        {
            var lookups = Enumerable.Range(0, numberOfLookups)
                .Select(i => new Lookup()
                {
                    SomeValue = RandomString()
                }).ToList();

            return new CategoryIndex<Lookup>()
            {
                Lookups = lookups
            };
        }

        public static CategoryIndex<Lookup> CreateCategoryIndex(out Lookup item1,
            out Lookup item2)
        {
            item1 = new();
            item2 = new();

            var lookups = item1.ToCollection(item2);

            return new CategoryIndex<Lookup>()
            {
                Lookups = lookups
            };
        }

        public static CategoryIndex<Lookup> CreateCategoryIndex(
            params string[] keys)
        {
            var lookups = keys.Select(k => new Lookup()
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