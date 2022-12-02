using FluentAssertions;
using Jcg.CategorizedRepository.Api.Exceptions;
using Jcg.CategorizedRepository.DataModelRepo.Strategies.imp;
using Jcg.CategorizedRepository.UnitTests.DataModelRepo.TestCommon;
using Testing.CommonV2.Mocks;
using Testing.CommonV2.Types;

namespace Jcg.CategorizedRepository.UnitTests.DataModelRepo.Strategies
{
    public class InitializeCategoryIndexStrategyTests
    {
        public InitializeCategoryIndexStrategyTests()
        {
            CategoryIndexFactory = new();
            UnitOfWork = new();


            Sut = new(
                CategoryIndexFactory.Object, UnitOfWork.Object);
        }


        private CategoryIndexFactoryMock CategoryIndexFactory { get; }

        private UnitOfWorkMock UnitOfWork { get; }

        private InitializeCategoryIndexStrategy<AggregateDatabaseModel,
            Lookup> Sut { get; }


        [Fact]
        public async Task CategoryIsInitialized_DelegatesToUnitOfWork()
        {
            // ************ ARRANGE ************

            UnitOfWork.SetupCategoryIndexIsInitialized(true);

            // ************ ACT ****************

            var result =
                await Sut.CategoryIsInitializedAsync(CancellationToken.None);

            // ************ ASSERT *************

            result.Should().BeTrue();
        }

        [Fact]
        public async Task Initialize_CategoryIndexIsAlreadyInitialized_Throws()
        {
            // ************ ARRANGE ************

            UnitOfWork.SetupCategoryIndexIsInitialized(true);

            // ************ ACT ****************

            Func<Task> fun = async () =>
            {
                await Sut.InitializeCategoryIndexes(CancellationToken.None);
            };

            // ************ ASSERT *************

            await fun.Should()
                .ThrowAsync<CategoryIndexIsAlreadyInitializedException>();
        }


        [Fact]
        public async Task
            Initialize_CreatesNonDeletedCategoryIndex_CreatesDeletedCategoryIndex_UpsertsBoth()
        {
            // ************ ARRANGE ************

            UnitOfWork.SetupCategoryIndexIsInitialized(false);

            // ************ ACT ****************

            await Sut.InitializeCategoryIndexes(CancellationToken.None);

            // ************ ASSERT *************

            UnitOfWork.VerifyUpsertDeletedItemsCategoryIndex(
                CategoryIndexFactory.Returns);

            UnitOfWork.VerifyUpsertNonDeletedItemsCategoryIndex(
                CategoryIndexFactory.Returns);
        }
    }
}