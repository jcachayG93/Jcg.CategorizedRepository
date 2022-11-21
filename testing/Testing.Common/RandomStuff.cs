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