using FluentAssertions;
using Jcg.CategorizedRepository.Api.Exceptions;
using Jcg.CategorizedRepository.DataModelRepo.Support.IndexManipulator;
using Testing.CommonV2.Mocks;
using Testing.CommonV2.Types;

namespace Jcg.CategorizedRepository.UnitTests.DataModelRepo.IndexManipulator
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
            Lookup> Sut { get; }


        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void
            Upsert_MapsAggregateToPayload_UpsertsLookupWherePayLoadIsMapperResultAndKeIsPassedValue(
                bool isInsert)
        {
            // ************ ARRANGE ************

            var aggregate = RandomAggregateDatabaseModel();

            var key = RandomString();

            var index = CreateCategoryIndex(isInsert ? RandomString() : key);

            // ************ ACT ****************

            Sut.Upsert(index, key, aggregate);

            // ************ ASSERT *************

            var result = index.Lookups.First(l =>
                l.Key == key);

            result.PayLoad.Should().Be(Mapper.Returns);
            result.IsDeleted.Should().BeFalse();
            result.Key.Should().Be(key);

            if (isInsert)
            {
                index.Lookups.Length.Should().Be(2);
            }
            else
            {
                index.Lookups.Length.Should().Be(1);
            }
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