using IntegrationTests.Common.Database;
using IntegrationTests.Common.Helpers;
using IntegrationTests.Common.Parts;
using IntegrationTests.Common.Types;
using Jcg.DataAccessRepositories;


namespace CategorizedRepository.IntegrationTests
{
    public abstract class TestBase
    {
        protected TestBase()
        {
            CategoryKey = new RepositoryIdentity(Guid.NewGuid());

            Database = new InMemoryDatabase();
        }

        private RepositoryIdentity CategoryKey { get; }

        private IInMemoryDatabase Database { get; }

        protected ICategorizedRepository<Customer, Lookup> CreateSut()
        {
            var aggregateMapper = new AggregateMapper();
            var aggregateToLookupMapper = new AggregateToLookupMapper();
            var lookupMapper = new LookupMapper();


            var client = new TransactionalDatabaseClient(Database);


            return CategorizedRepositoryFactory
                .Create(CategoryKey,
                    client,
                    aggregateMapper,
                    aggregateToLookupMapper,
                    lookupMapper);
        }

        protected async Task<ICategorizedRepository<Customer, Lookup>>
            CreateSutAndInitializeIndex()
        {
            var sut = CreateSut();

            await sut.InitializeCategoryIndexAsync(CancellationToken.None);

            return sut;
        }

        protected async Task<ICategorizedRepository<Customer, Lookup>>
            CreateSutWithCustomer(Customer customer)
        {
            var sut = CreateSut();

            await sut.UpsertAsync(customer.Id.ToKey(), customer,
                CancellationToken.None);

            return sut;
        }

        protected async Task InitializeCategoryIndexAsync()
        {
            var sut = await CreateSutAndInitializeIndex();

            await sut.CommitChangesAsync(CancellationToken.None);
        }

        protected async Task AddAgregateAsync(Customer aggregate)
        {
            var sut = await CreateSutWithCustomer(aggregate);

            await sut.CommitChangesAsync(CancellationToken.None);
        }
    }
}