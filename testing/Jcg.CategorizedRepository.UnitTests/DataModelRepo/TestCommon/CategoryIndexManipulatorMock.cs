using Jcg.CategorizedRepository.Api;
using Jcg.CategorizedRepository.DataModelRepo.Support.IndexManipulator;
using Moq;
using Testing.CommonV2.Types;

namespace Jcg.CategorizedRepository.UnitTests.DataModelRepo.TestCommon
{
    internal class CategoryIndexManipulatorMock
    {
        public CategoryIndexManipulatorMock()
        {
            _moq = new();
        }

        public ICategoryIndexManipulator<AggregateDatabaseModel,
            LookupDatabaseModel> Object => _moq.Object;

        public void VerifyUpsert(
            CategoryIndex<LookupDatabaseModel> nonDeletedCategoryIndex,
            AggregateDatabaseModel aggregate)
        {
            _moq.Verify(s => s.Upsert(nonDeletedCategoryIndex,
                aggregate));
        }

        public void VerifyDelete(
            CategoryIndex<LookupDatabaseModel> nonDeletedCategoryIndex,
            CategoryIndex<LookupDatabaseModel> deletedCategoryIndex,
            string key)
        {
            _moq.Verify(s =>
                s.Delete(nonDeletedCategoryIndex, deletedCategoryIndex, key,
                    It.IsAny<DateTime>()));
        }

        public void VerifyRestore(
            CategoryIndex<LookupDatabaseModel> nonDeletedCategoryIndex,
            CategoryIndex<LookupDatabaseModel> deletedCategoryIndex,
            string key)
        {
            _moq.Verify(s =>
                s.Restore(nonDeletedCategoryIndex, deletedCategoryIndex, key));
        }

        private readonly Mock<ICategoryIndexManipulator<AggregateDatabaseModel,
            LookupDatabaseModel>> _moq;
    }
}