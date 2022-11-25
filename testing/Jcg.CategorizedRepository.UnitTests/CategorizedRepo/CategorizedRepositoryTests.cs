using FluentAssertions;
using Jcg.CategorizedRepository.Api;
using Jcg.CategorizedRepository.CategorizedRepo;
using Testing.CommonV2.Mocks;
using Testing.CommonV2.Types;

namespace Jcg.CategorizedRepository.UnitTests.CategorizedRepo
{
    public class CategorizedRepositoryTests
    {
        public CategorizedRepositoryTests()
        {
            AggregateMapper = new();


            DataModelRepository = new();

            Sut = new(
                AggregateMapper.Object,
                DataModelRepository.Object);


            Key = new RepositoryIdentity(Guid.NewGuid());


            Aggregate = new();
        }

        private AggregateMapperMock AggregateMapper { get; }


        private DataModelRepositoryMock DataModelRepository { get; }

        private CategorizedRepository<Aggregate, AggregateDatabaseModel, Lookup>
            Sut { get; }

        public RepositoryIdentity Key { get; }

        public Aggregate Aggregate { get; }

        [Fact]
        public async Task
            InitializeCategoryIndex_DelegatesToDataModelRepository()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            await Sut.InitializeCategoryIndexAsync(CancellationToken.None);

            // ************ ASSERT *************

            DataModelRepository.VerifyInitializeCategory();
        }

        [Fact]
        public async Task
            GetAggregate_GetsAggregateFromDataModelRepository_MapsToAggregate_ReturnsMapperResult()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            var result =
                await Sut.GetAggregateAsync(Key, CancellationToken.None);

            // ************ ASSERT *************

            DataModelRepository.VerifyGetAggregate(Key.Value);

            AggregateMapper.VerifyToAggregate(DataModelRepository
                .GetAggregateReturns);

            result.Should().BeSameAs(AggregateMapper.ToAggregateReturns);
        }

        [Fact]
        public async Task
            Upsert_MapsAggregateToAggregateDataModel_UpsertsResultToDataModelRepository()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            await Sut.UpsertAsync(Key, Aggregate, CancellationToken.None);

            // ************ ASSERT *************

            AggregateMapper.VerifyToDatabaseModel(Aggregate);

            DataModelRepository.VerifyUpsert(AggregateMapper
                .ToDatabaseModelReturns);
        }

        [Fact]
        public async Task
            LookupNonDeleted_GetsNonDeletedLookupsFromDataModelRepository_ReturnsLookupsFromResult()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            var result =
                await Sut.LookupNonDeletedAsync(CancellationToken.None);

            // ************ ASSERT *************

            DataModelRepository.VerifyLookupNonDeleted();


            result.Should().BeSameAs(DataModelRepository.LookupNonDeletedReturns
                .Lookups);
        }

        [Fact]
        public async Task
            LookupDeleted_GetsDeletedLookupsFromDataModelRepository_ReturnsLookupsFromResult()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            var result = await Sut.LookupDeletedAsync(CancellationToken.None);

            // ************ ASSERT *************

            DataModelRepository.VerifyLookupDeleted();

            result.Should()
                .BeSameAs(DataModelRepository.LookupDeletedReturns.Lookups);
        }

        [Fact]
        public async Task Delete_DelegatesToDataModelRepository()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            await Sut.DeleteAsync(Key, CancellationToken.None);

            // ************ ASSERT *************

            DataModelRepository.VerifyDelete(Key.Value);
        }

        [Fact]
        public async Task Restore_DelegatesToDataModelRepository()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            await Sut.RestoreAsync(Key, CancellationToken.None);

            // ************ ASSERT *************

            DataModelRepository.VerifyRestore(Key.Value);
        }

        [Fact]
        public async Task Commit_DelegatesToDataModelRepository()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            await Sut.CommitChangesAsync(CancellationToken.None);

            // ************ ASSERT *************

            DataModelRepository.VerifyCommit();
        }
    }
}