using FluentAssertions;
using Jcg.Repositories.Api.Exceptions;
using Support.DataModelRepository.Strategies;
using Support.DataModelRepository.Strategies.imp;
using Support.DataModelRepository.UnitTests.TestCommon;
using Testing.Common.Mocks;
using Testing.Common.Types;

namespace Support.DataModelRepository.UnitTests.Strategies
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
            LookupDatabaseModel> Sut { get; }


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