using IntegrationTests.Common.Database;
using IntegrationTests.Common.Helpers;
using IntegrationTests.Common.Parts;
using IntegrationTests.Common.Types;
using Jcg.CategorizedRepository.Api;


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

        protected ICategorizedRepository<Customer, CustomerLookupDataModel> CreateSut()
        {
            var aggregateMapper = new AggregateMapper();
            var aggregateToLookupMapper = new AggregateToLookupMapper();


            var client = new TransactionalDatabaseClient(Database);


            return CategorizedRepositoryFactory
                .Create(CategoryKey,
                    client,
                    aggregateMapper,
                    aggregateToLookupMapper);
        }

        protected async Task<ICategorizedRepository<Customer, CustomerLookupDataModel>>
            CreateSutAndInitializeIndex()
        {
            var sut = CreateSut();

            await sut.InitializeCategoryIndexAsync(CancellationToken.None);

            return sut;
        }

        protected async Task<ICategorizedRepository<Customer, CustomerLookupDataModel>>
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