using Testing.Common.Doubles;
using Testing.Common.Types;

namespace Support.UnitOfWork.IntegrationTests
{
    public abstract class TestBase
    {
        public TestBase()
        {
            DataSource = new();

            DatabaseClient = new TransactionalDatabaseClient(DataSource);

            DeletedCategoryIndexKey = RandomString();

            NonDeletedCategoryIndexKey = RandomString();

            UnitOfWorkFactory = new UnitOfWorkFactory();
        }

        public string DeletedCategoryIndexKey { get; }

        public string NonDeletedCategoryIndexKey { get; }

        private TransactionalDatabaseClient DatabaseClient { get; }

        private UnitOfWorkFactory UnitOfWorkFactory { get; }

        protected InMemoryDataSource DataSource { get; }


        /// <summary>
        ///     Creates a unit of work instance. Each one created with this method shacer the same underlying database client,
        ///     database and category keys
        /// </summary>
        /// <returns></returns>
        internal IUnitOfWork<AggregateDatabaseModel, LookupDatabaseModel>
            CreateSut()
        {
            return UnitOfWorkFactory.Create(NonDeletedCategoryIndexKey,
                DeletedCategoryIndexKey, DatabaseClient);
        }
    }
}