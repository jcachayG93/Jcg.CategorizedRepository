using Jcg.CategorizedRepository.Api;
using Moq;
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
                SomeValue = RandomString()
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

            var lookupDtos = lookups.Select(l =>
                new LookupDto<Lookup>
                {
                    PayLoad = l
                }).ToArray();

            return new CategoryIndex<Lookup>()
            {
                Lookups = lookupDtos
            };
        }


        public static CategoryIndex<Lookup> CreateCategoryIndex(
            params string[] keys)
        {
            var dtos = keys.Select(k =>
                new LookupDto<Lookup>
                {
                    Key = k,
                    IsDeleted = false,
                    DeletedTimeStamp = "",
                    PayLoad = new Lookup()
                }).ToArray();


            return new()
            {
                Lookups = dtos
            };
        }


        public static AggregateDatabaseModel CreateAggregateDatabaseModel(
            string key)
        {
            return new()
            {
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