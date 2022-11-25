using Jcg.CategorizedRepository.Api;
using Jcg.CategorizedRepository.UoW;
using Moq;
using Testing.CommonV2.Types;

namespace Testing.CommonV2.Mocks
{
    internal class UnitOfWorkMock
    {
        public UnitOfWorkMock()
        {
            _moq = new();
            GetNonDeletedItemsCategoryIndexReturns = RandomCategoryIndex();
            GetDeletedItemsCategoryIndexReturns = RandomCategoryIndex();
            GetAggregateReturns = RandomAggregateDatabaseModel();

            _moq.Setup(s =>
                    s.GetNonDeletedItemsCategoryIndex(AnyCt()).Result)
                .Returns(GetNonDeletedItemsCategoryIndexReturns);

            _moq.Setup(s =>
                    s.GetDeletedItemsCategoryIndex(AnyCt()).Result)
                .Returns(GetDeletedItemsCategoryIndexReturns);

            _moq.Setup(s =>
                    s.GetAggregateAsync(AnyString(), AnyCt()).Result)
                .Returns(GetAggregateReturns);

            SetupCategoryIndexIsInitialized(true);
        }


        public IUnitOfWork<AggregateDatabaseModel, Lookup>
            Object => _moq.Object;


        public CategoryIndex<Lookup>
            GetNonDeletedItemsCategoryIndexReturns { get; }


        public CategoryIndex<Lookup>
            GetDeletedItemsCategoryIndexReturns { get; }

        public AggregateDatabaseModel GetAggregateReturns { get; }

        public void VerifyGetNonDeletedCategoryIndex()
        {
            _moq.Verify(s =>
                s.GetNonDeletedItemsCategoryIndex(AnyCt()));
        }

        public void VerifyGetDeletedCategoryIndex()
        {
            _moq.Verify(s =>
                s.GetDeletedItemsCategoryIndex(AnyCt()));
        }

        public void VerifyUpsertDeletedItemsCategoryIndex(
            CategoryIndex<Lookup> deletedItemsCategoryIndex)
        {
            _moq.Verify(s =>
                s.UpsertDeletedItemsCategoryIndex(deletedItemsCategoryIndex,
                    AnyCt()));
        }

        public void VerifyUpsertAggregate(string key,
            AggregateDatabaseModel aggregate)
        {
            _moq.Verify(s =>
                s.UpsertAggregateAsync(key, aggregate, AnyCt()));
        }

        public void VerifyUpsertNonDeletedItemsCategoryIndex(
            CategoryIndex<Lookup> nonDeletedItemsCategoryIndex)
        {
            _moq.Verify(s =>
                s.UpsertNonDeletedItemsCategoryIndex(
                    nonDeletedItemsCategoryIndex, AnyCt()));
        }

        public void VerifyGetAggregate(string key)
        {
            _moq.Verify(s => s.GetAggregateAsync(key, AnyCt()));
        }


        public void VerifyCommitChanges(CancellationToken cancellationToken)
        {
            _moq.Verify(s => s.CommitChangesAsync(cancellationToken));
        }

        public void SetupCategoryIndexIsInitialized(bool returns)
        {
            _moq.Setup(s => s.CategoryIndexIsInitializedAsync(AnyCt()).Result)
                .Returns(returns);
        }

        private readonly
            Mock<IUnitOfWork<AggregateDatabaseModel, Lookup>> _moq;
    }
}