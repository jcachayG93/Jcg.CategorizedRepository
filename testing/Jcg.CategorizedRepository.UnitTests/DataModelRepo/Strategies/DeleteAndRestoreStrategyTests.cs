using Jcg.CategorizedRepository.DataModelRepo.Strategies.imp;
using Jcg.CategorizedRepository.UnitTests.DataModelRepo.TestCommon;
using Testing.CommonV2.Mocks;
using Testing.CommonV2.Types;

namespace Jcg.CategorizedRepository.UnitTests.DataModelRepo.Strategies
{
    public class DeleteAndRestoreStrategyTests
    {
        public DeleteAndRestoreStrategyTests()
        {
            IndexManipulator = new();
            UnitOfWork = new();
            Sut = new(IndexManipulator.Object, UnitOfWork.Object);
        }

        private CategoryIndexManipulatorMock IndexManipulator { get; }

        private UnitOfWorkMock UnitOfWork { get; }

        private DeleteAndRestoreStrategy<AggregateDatabaseModel,
            Lookup> Sut { get; }


        [Fact]
        public async Task
            Delete_GetsBothCategoryIndexesFromUnitOfWork_UsesIndexManipulatorToDelete_UpsertsBothCategoryIndexesToUnitOfWork()
        {
            // ************ ARRANGE ************

            var key = Guid.NewGuid();

            // ************ ACT ****************

            await Sut.DeleteAsync(key, CancellationToken.None);

            // ************ ASSERT *************

            UnitOfWork.VerifyGetNonDeletedCategoryIndex();
            UnitOfWork.VerifyGetDeletedCategoryIndex();

            IndexManipulator.VerifyDelete(
                UnitOfWork.GetNonDeletedItemsCategoryIndexReturns,
                UnitOfWork.GetDeletedItemsCategoryIndexReturns,
                key.ToString());

            UnitOfWork.VerifyUpsertNonDeletedItemsCategoryIndex(UnitOfWork
                .GetNonDeletedItemsCategoryIndexReturns);

            UnitOfWork.VerifyUpsertDeletedItemsCategoryIndex(UnitOfWork
                .GetDeletedItemsCategoryIndexReturns);
        }

        [Fact]
        public async Task
            Restore_GetsBothCategoryIndexesFromUnitOfWork_UsesIndexManipulatorToRestore_UpsertsBothCategoryIndexesToUnitOfWork()
        {
            // ************ ARRANGE ************

            var key = Guid.NewGuid();

            // ************ ACT ****************

            await Sut.RestoreAsync(key, CancellationToken.None);

            // ************ ASSERT *************

            UnitOfWork.VerifyGetNonDeletedCategoryIndex();
            UnitOfWork.VerifyGetDeletedCategoryIndex();

            IndexManipulator.VerifyRestore(
                UnitOfWork.GetNonDeletedItemsCategoryIndexReturns,
                UnitOfWork.GetDeletedItemsCategoryIndexReturns,
                key.ToString());

            UnitOfWork.VerifyUpsertNonDeletedItemsCategoryIndex(UnitOfWork
                .GetNonDeletedItemsCategoryIndexReturns);

            UnitOfWork.VerifyUpsertDeletedItemsCategoryIndex(UnitOfWork
                .GetDeletedItemsCategoryIndexReturns);
        }
    }
}