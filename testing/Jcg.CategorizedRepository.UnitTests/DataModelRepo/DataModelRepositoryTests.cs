using FluentAssertions;
using Jcg.CategorizedRepository.DataModelRepo;
using Jcg.CategorizedRepository.UnitTests.DataModelRepo.TestCommon;
using Testing.CommonV2.Types;

namespace Jcg.CategorizedRepository.UnitTests.DataModelRepo
{
    public class DataModelRepositoryTests
    {
        public DataModelRepositoryTests()
        {
            InitializeCategoryIndexStrategy = new();

            QueryStrategy = new();

            UpsertAggregateStrategy = new();

            DeleteAndRestoreStrategy = new();

            CommitStrategy = new();

            Sut = new(
                InitializeCategoryIndexStrategy.Object,
                QueryStrategy.Object,
                UpsertAggregateStrategy.Object,
                DeleteAndRestoreStrategy.Object,
                CommitStrategy.Object);
        }

        private InitializeCategoryIndexesMock InitializeCategoryIndexStrategy
        {
            get;
        }

        private QueryStategyMock QueryStrategy { get; }

        private UpsertAggregateStatregyMock UpsertAggregateStrategy { get; }

        private DeleteAndRestoreStrategyMock DeleteAndRestoreStrategy { get; }

        private CommitStategyMock CommitStrategy { get; }

        private DataModelRepository<AggregateDatabaseModel, Lookup>
            Sut { get; }


        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task
            CategoryIsInitialized_DelegatesToInitializeCategoryStrategy(
                bool expected)
        {
            // ************ ARRANGE ************

            InitializeCategoryIndexStrategy
                .SetupCategoryIsInitialized(expected);

            // ************ ACT ****************

            var result =
                await Sut.CategoryIsAlreadyInitializedAsync(CancellationToken
                    .None);

            // ************ ASSERT *************

            result.Should().Be(expected);
        }

        [Fact]
        public async Task InitializeCategory_DelegatesToStrategy()
        {
            // ************ ARRANGE ************

            var cs = new CancellationToken();

            // ************ ACT ****************

            await Sut.InitializeCategoryIndexes(cs);

            // ************ ASSERT *************

            InitializeCategoryIndexStrategy.VerifyInitializeCategoryIndexes(cs);
        }


        [Fact]
        public async Task GetAggregate_DelegatesToStrategy()
        {
            // ************ ARRANGE ************

            var key = Guid.NewGuid();

            // ************ ACT ****************

            var result =
                await Sut.GetAggregateAsync(key, CancellationToken.None);

            // ************ ASSERT *************

            QueryStrategy.VerifyGetAggregate(key);

            result.Should().Be(QueryStrategy.GetAggregateReturns);
        }


        [Fact]
        public async Task LookupNonDeleted_DelegatesToStrategy()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            var result =
                await Sut.LookupNonDeletedAsync(CancellationToken.None);

            // ************ ASSERT *************

            result.Should().BeSameAs(QueryStrategy.LookupNonDeletedReturns);
        }

        [Fact]
        public async Task LookupDeleted_DelegatesToStrategy()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            var result =
                await Sut.LookupDeletedAsync(CancellationToken.None);

            // ************ ASSERT *************

            result.Should().BeSameAs(QueryStrategy.LookupDeletedReturns);
        }


        [Fact]
        public async Task Upsert_DelegatesToStrategy()
        {
            // ************ ARRANGE ************


            var aggregate = RandomAggregateDatabaseModel();

            var key = RandomString();

            // ************ ACT ****************

            await Sut.UpsertAsync(key, aggregate, CancellationToken.None);

            // ************ ASSERT *************

            UpsertAggregateStrategy
                .VerifyUpsert(key, aggregate);
        }


        [Fact]
        public async Task Delete_DelegatesToStrategy()
        {
            // ************ ARRANGE ************

            var key = Guid.NewGuid();

            // ************ ACT ****************

            await Sut.DeleteAsync(key, CancellationToken.None);

            // ************ ASSERT *************

            DeleteAndRestoreStrategy.VerifyDelete(key);
        }

        [Fact]
        public async Task Restore_DelegatesToStrategy()
        {
            // ************ ARRANGE ************

            var key = Guid.NewGuid();

            // ************ ACT ****************

            await Sut.RestoreAsync(key, CancellationToken.None);

            // ************ ASSERT *************

            DeleteAndRestoreStrategy.VerifyRestore(key);
        }


        [Fact]
        public async Task Commit_DelegatesToStrategy()
        {
            // ************ ARRANGE ************

            var cs = new CancellationToken();

            // ************ ACT ****************

            await Sut.CommitChangesAsync(cs);

            // ************ ASSERT *************

            CommitStrategy.VerifyCommit(cs);
        }
    }
}