using FluentAssertions;
using IntegrationTests.Common.Database;
using Xunit;

namespace IntegrationTests.Common.Parts
{
    public class TransactionalDatabaseClientTests
    {
        public TransactionalDatabaseClientTests()
        {
            _database = new InMemoryDatabase();
        }

        private TransactionalDatabaseClient CreateSut()
        {
            return new(_database);
        }


        [Fact]
        public async Task GetAggregate_NoAggregate_ReturnsNull()
        {
            // ************ ARRANGE ************

            var sut = CreateSut();

            // ************ ACT ****************

            var result =
                await sut.GetAggregateAsync(RandomString(),
                    CancellationToken.None);

            // ************ ASSERT *************

            result.Should().BeNull();
        }


        [Fact]
        public async Task GetCategoryIndex_NoCategoryIndex_ReturnsNull()
        {
            // ************ ARRANGE ************

            var sut = CreateSut();

            // ************ ACT ****************

            var result =
                await sut.GetCategoryIndex(RandomString(),
                    CancellationToken.None);

            // ************ ASSERT *************

            result.Should().BeNull();
        }


        [Fact]
        public async Task Commit_CommitsCategoryIndexesToDatabase()
        {
            // ************ ARRANGE ************

            var sut = CreateSut();

            var aggregate = RandomCustomerDataModel(out var key);

            aggregate.Name = "juan";

            await sut.UpsertAggregateAsync("", aggregate,
                CancellationToken.None);

            await sut.CommitTransactionAsync(CancellationToken.None);

            var otherSut = CreateSut();

            // ************ ACT ****************

            var result =
                await otherSut.GetAggregateAsync(key, CancellationToken.None);

            // ************ ASSERT *************

            result.Payload.Name.Should().Be("juan");
        }


        [Fact]
        public async Task Commit_CommitsAggregatesToDatabase()
        {
            // ************ ARRANGE ************

            var sut = CreateSut();

            var index = RandomCategoryIndex("juan");

            var key = RandomString();

            await sut.UpsertCategoryIndex(key, "", index,
                CancellationToken.None);

            await sut.CommitTransactionAsync(CancellationToken.None);


            // ************ ACT ****************

            var resut = await sut.GetCategoryIndex(key, CancellationToken.None);

            // ************ ASSERT *************

            resut.Payload.Lookups.First().PayLoad.CustomerName.Should()
                .Be("juan");
        }


        [Fact]
        public async Task ChecksOptimisticConcurrency()
        {
            // ************ ARRANGE ************

            var item = RandomCustomerDataModel(out var key);

            var sut1 = CreateSut();
            var sut2 = CreateSut();

            await sut1.UpsertAggregateAsync("", item, CancellationToken.None);
            await sut1.CommitTransactionAsync(CancellationToken.None);

            var aggregate =
                await sut1.GetAggregateAsync(key, CancellationToken.None);

            await sut2.UpsertAggregateAsync(aggregate.Etag, aggregate.Payload,
                CancellationToken.None);

            await sut2.CommitTransactionAsync(CancellationToken.None);

            // ************ ACT ****************

            var fun = async () =>
            {
                await sut1.UpsertAggregateAsync(aggregate.Etag,
                    aggregate.Payload,
                    CancellationToken.None);

                await sut1.CommitTransactionAsync(CancellationToken.None);
            };

            // ************ ASSERT *************

            await fun.Should().ThrowAsync<OptimisticConcurrencyException>();
        }

        private readonly IInMemoryDatabase _database;
    }
}