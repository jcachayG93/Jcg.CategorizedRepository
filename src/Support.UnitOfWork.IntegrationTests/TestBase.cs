using Testing.Common.Doubles;
using Testing.Common.Types;

namespace Support.UnitOfWork.IntegrationTests
{
    public abstract class TestBase
    {
        public TestBase()
        {
            DataSource = new();

            var databaseClient = new TransactionalDatabaseClient(DataSource);

            DeletedCategoryIndexKey = RandomString();

            NonDeletedCategoryIndexKey = RandomString();

            Sut = (new UnitOfWorkFactory()).Create(
                NonDeletedCategoryIndexKey, 
                DeletedCategoryIndexKey, 
                databaseClient);
        }

        public string DeletedCategoryIndexKey { get; }

        public string NonDeletedCategoryIndexKey { get; }

        protected InMemoryDataSource DataSource { get; }

   

        internal IUnitOfWorkImp<AggregateDatabaseModel, LookupDatabaseModel> Sut { get; }
    }
}