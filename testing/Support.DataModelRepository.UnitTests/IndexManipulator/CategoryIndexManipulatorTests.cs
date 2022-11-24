using Common.Api.Exceptions;
using FluentAssertions;
using Support.DataModelRepository.Support.IndexManipulator;
using Testing.Common.Mocks;
using Testing.Common.Types;

namespace Support.DataModelRepository.UnitTests.IndexManipulator
{
    public class CategoryIndexManipulatorTests
    {
        public CategoryIndexManipulatorTests()
        {
            Mapper = new();

            Sut = new(Mapper.Object);
        }

        private AggregateToLookupMapperMock Mapper { get; }

        private CategoryIndexManipulator<AggregateDatabaseModel,
            LookupDatabaseModel> Sut { get; }


        [Fact]
        public void Upsert_MapsAggregate()
        {
            // ************ ARRANGE ************

            var aggregate = RandomAggregateDatabaseModel();

            // ************ ACT ****************

            Sut.Upsert(RandomCategoryIndex(), aggregate);

            // ************ ASSERT *************

            Mapper.VerifyToLookup(aggregate);
        }


        [Fact]
        public void
            Upsert_LookupForKeyExists_ReplacesPayload_KeySetToAggregateKey()
        {
            // ************ ARRANGE ************

            var index = CreateCategoryIndex("k1");

            var aggregate = CreateAggregateDatabaseModel("k1");

            // ************ ACT ****************

            Sut.Upsert(index, aggregate);

            // ************ ASSERT *************

            index.Lookups.Count().Should().Be(1);

            var lookup = index.Lookups.First();

            lookup.Should().BeSameAs(Mapper.Returns);

            lookup.Key.Should().Be("k1");

            lookup.IsDeleted.Should().BeFalse();
        }


        [Fact]
        public void
            Upsert_LookupForKeyDoesNotExist_AddsPayload_KeysetToAggregateKey()
        {
            // ************ ARRANGE ************

            var index = CreateCategoryIndex();

            var aggregate = CreateAggregateDatabaseModel("k1");

            // ************ ACT ****************

            Sut.Upsert(index, aggregate);

            // ************ ASSERT *************

            index.Lookups.Count().Should().Be(1);

            var lookup = index.Lookups.First();

            lookup.Should().BeSameAs(Mapper.Returns);

            lookup.Key.Should().Be("k1");

            lookup.IsDeleted.Should().BeFalse();
        }


        [Fact]
        public void Delete_NoMatch_Throws()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            var act = () =>
            {
                Sut.Delete(RandomCategoryIndex(),
                    RandomCategoryIndex(), RandomString(), DateTime.Now);
            };

            // ************ ASSERT *************

            act.Should().Throw<LookupNotFoundInCategoryIndexException>();
        }


        [Fact]
        public void
            Delete_MovesMatchingLookupFromNonDeletedToDeleted_SetsIsDeletedAndTimeStamp()
        {
            // ************ ARRANGE ************

            var nonDeleted = CreateCategoryIndex("k1");

            var deleted = CreateCategoryIndex();

            var timeStamp = DateTime.Now;

            var itemToDelete = nonDeleted.Lookups.First();

            // ************ ACT ****************

            Sut.Delete(nonDeleted, deleted, "k1", timeStamp);

            // ************ ASSERT *************

            nonDeleted.Lookups.Any().Should().BeFalse();

            var result = deleted.Lookups.First();

            result.IsDeleted.Should().BeTrue();
            result.DeletedTimeStamp.Should().Be(timeStamp.ToString("o"));
            result.Should().BeSameAs(itemToDelete);
        }


        [Fact]
        public void Restore_NoMatch_Throws()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            var act = () =>
            {
                Sut.Restore(RandomCategoryIndex(),
                    RandomCategoryIndex(), RandomString());
            };

            // ************ ASSERT *************

            act.Should().Throw<LookupNotFoundInCategoryIndexException>();
        }


        [Fact]
        public void
            Restore_MovesMatchingLookupFromDeletedToNonDeleted_SetsIsDeletedToFalseAndTimeStampToMinValue()
        {
            // ************ ARRANGE ************

            var nonDeleted = CreateCategoryIndex();

            var deleted = CreateCategoryIndex("k1");

            var itemToRestore = deleted.Lookups.First();

            // ************ ACT ****************

            Sut.Restore(nonDeleted, deleted, "k1");

            // ************ ASSERT *************

            deleted.Lookups.Any().Should().BeFalse();

            var result = nonDeleted.Lookups.First();

            result.Should().BeSameAs(itemToRestore);

            result.IsDeleted.Should().BeFalse();

            result.DeletedTimeStamp.Should()
                .Be(DateTime.MinValue.ToString("o"));
        }
    }
}