using Moq;
using Support.UnitOfWork.Api;
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
            return new();
        }

        public static CategoryIndex<LookupDatabaseModel> RandomCategoryIndex()
        {
            return new();
        }

        public static CategoryIndex<LookupDatabaseModel> CreateCategoryIndex(int numberOfLookups)
        {
            var lookups = Enumerable.Range(0, numberOfLookups)
                .Select(i => new LookupDatabaseModel(){SomeValue = RandomString()}).ToList();

            return new CategoryIndex<LookupDatabaseModel>() {Lookups = lookups};
        }

        public static AggregateDatabaseModel CreateAggregateDatabaseModel(out string value)
        {
            value = RandomString();

            return new() {SomeValue = value};
        }


        public static CancellationToken AnyCt()
        {
            return It.IsAny<CancellationToken>();
        }

        public static string AnyString()
        {
            return It.IsAny<string>();
        }
    }
}