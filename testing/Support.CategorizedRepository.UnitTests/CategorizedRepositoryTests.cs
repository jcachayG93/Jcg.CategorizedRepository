using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Api;
using FluentAssertions;
using Support.CategorizedRepository.UnitTests.TestCommon;
using Testing.Common.Mocks;
using Testing.Common.Types;

namespace Support.CategorizedRepository.UnitTests
{
    public class CategorizedRepositoryTests
    {
        public CategorizedRepositoryTests()
        {
            AggregateMapper = new();

            AggregateToLookupMapper = new();

            LookupMapper = new();

            DataModelRepository = new();

            Sut = new(
                AggregateMapper.Object, 
                AggregateToLookupMapper.Object,
                LookupMapper.Object,
                DataModelRepository.Object);

          

            Key = new RepositoryIdentity(Guid.NewGuid());

            AggregateMapper = new();
        }
        private AggregateMapperMock AggregateMapper { get; }

        private AggregateToLookupMapperMock AggregateToLookupMapper { get; }

        private LookupMapperAdapterMock LookupMapper { get; }

        private DataModelRepositoryMock DataModelRepository { get; }

        private CategorizedRepository<Aggregate, AggregateDatabaseModel, Lookup, LookupDatabaseModel> Sut { get; }

        public RepositoryIdentity Key { get; }

        public Aggregate Aggregate { get; }

        [Fact]
        public async Task InitializeCategoryIndex_DelegatesToDataModelRepository()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            await Sut.InitializeCategoryIndexAsync(CancellationToken.None);

            // ************ ASSERT *************

            DataModelRepository.VerifyInitializeCategory();
        }

        [Fact]
        public async Task GetAggregate_GetsAggregateFromDataModelRepository_MapsToAggregate_ReturnsMapperResult()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            var result = await Sut.GetAggregateAsync(Key, CancellationToken.None);

            // ************ ASSERT *************

            DataModelRepository.VerifyGetAggregate(Key.Value);

            AggregateMapper.VerifyToAggregate(DataModelRepository.GetAggregateReturns);

            result.Should().BeSameAs(AggregateMapper.ToAggregateReturns);
        }

        [Fact]
        public async Task Upsert_MapsAggregateToAggregateDataModel_UpsertsResultToDataModelRepository()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            await Sut.UpsertAsync(Key, Aggregate, CancellationToken.None);

            // ************ ASSERT *************

            AggregateMapper.VerifyToDatabaseModel(Aggregate);

            DataModelRepository.VerifyUpsert(AggregateMapper.ToDatabaseModelReturns);
        }

        [Fact]
        public async Task LookupNonDeleted_GetsNonDeletedLookupsFromDataModelRepository_MapsResult_ReturnsMapperResult()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            var result = await Sut.LookupNonDeletedAsync(CancellationToken.None);

            // ************ ASSERT *************

            DataModelRepository.VerifyLookupNonDeleted();

            LookupMapper.VerifyMap(DataModelRepository.LookupNonDeletedReturns);

            result.Should().BeSameAs(LookupMapper.MapReturns);
        }

        [Fact]
        public async Task LookupDeleted_GetsDeletedLookupsFromDataModelRepository_MapsResult_ReturnsMapperResult()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            var result = await Sut.LookupDeletedAsync(CancellationToken.None);

            // ************ ASSERT *************

            DataModelRepository.VerifyLookupDeleted();

            LookupMapper.VerifyMap(DataModelRepository.LookupDeletedReturns);

            result.Should().BeSameAs(LookupMapper.MapReturns);
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
