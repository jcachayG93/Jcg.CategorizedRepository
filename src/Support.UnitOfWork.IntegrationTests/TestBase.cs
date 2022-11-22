using Testing.Common.Doubles;

namespace Support.UnitOfWork.IntegrationTests
{
    public abstract class TestBase
    {
        public TestBase()
        {
            DatabaseClient = new(new());
        }

        protected TransactionalDatabaseClient DatabaseClient { get; }

        internal UnitOfWorkFactory UnitOfWorkFactory { get; }
    }
}