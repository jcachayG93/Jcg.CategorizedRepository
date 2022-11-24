using CategorizedRepository.Factories;
using Common.Api;
using IntegrationTests.Common.Database;
using IntegrationTests.Common.Helpers;
using IntegrationTests.Common.Parts;
using IntegrationTests.Common.Types;

namespace CategorizedRepository.IntegrationTests
{
    public abstract class TestBase
    {
        protected TestBase()
        {
            CategoryKey = new RepositoryIdentity(Guid.NewGuid());
        }

        public RepositoryIdentity CategoryKey { get; }

        protected ICategorizedRepository<Customer, Lookup> CreateSut()
        {
            var aggregateMapper = new AggregateMapper();
            var aggregateToLookupMapper = new AggregateToLookupMapper();
            var lookupMapper = new LookupMapper();

            var database = new InMemoryDatabase();

            var client = new TransactionalDatabaseClient(database);


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
            var sut = await CreateSutAndInitializeIndex();

            await sut.UpsertAsync(customer.Id.ToKey(), customer,
                CancellationToken.None);

            return sut;
        }
    }
}