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
            Lookup> Object => _moq.Object;

        [Obsolete]
// TODO: R200 Remove
        public void VerifyUpsert(
            CategoryIndex<Lookup> nonDeletedCategoryIndex,
            AggregateDatabaseModel aggregate)
        {
            throw new NotImplementedException();
        }

        public void VerifyUpsert(
            CategoryIndex<Lookup> nonDeletedCategoryIndex,
            string key,
            AggregateDatabaseModel aggregate)
        {
            _moq.Verify(s => s.Upsert(
                nonDeletedCategoryIndex,
                key,
                aggregate));
        }

        public void VerifyDelete(
            CategoryIndex<Lookup> nonDeletedCategoryIndex,
            CategoryIndex<Lookup> deletedCategoryIndex,
            string key)
        {
            _moq.Verify(s =>
                s.Delete(nonDeletedCategoryIndex, deletedCategoryIndex, key,
                    It.IsAny<DateTime>()));
        }

        public void VerifyRestore(
            CategoryIndex<Lookup> nonDeletedCategoryIndex,
            CategoryIndex<Lookup> deletedCategoryIndex,
            string key)
        {
            _moq.Verify(s =>
                s.Restore(nonDeletedCategoryIndex, deletedCategoryIndex, key));
        }

        private readonly Mock<ICategoryIndexManipulator<AggregateDatabaseModel,
            Lookup>> _moq;
    }
}