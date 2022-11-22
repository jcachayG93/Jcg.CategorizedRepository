using Testing.Common.Doubles;
using Testing.Common.Types;

namespace Support.UnitOfWork.IntegrationTests
{
    public abstract class TestBase
    {
        public TestBase()
        {
            DatabaseClient = new(new());

            Sut = (new UnitOfWorkFactory()).Create(
                "non-deleted-index", 
                "deleted-index", 
                DatabaseClient);
        }

        protected TransactionalDatabaseClient DatabaseClient { get; }

        internal IUnitOfWorkImp<AggregateDatabaseModel, LookupDatabaseModel> Sut { get; }
    }
}