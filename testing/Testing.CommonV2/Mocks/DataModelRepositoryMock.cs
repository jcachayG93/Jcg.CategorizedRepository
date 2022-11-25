using Jcg.CategorizedRepository.Api;
using Jcg.CategorizedRepository.DataModelRepo;
using Moq;
using Testing.CommonV2.Types;

namespace Testing.CommonV2.Mocks
{
    internal class DataModelRepositoryMock
    {
        public DataModelRepositoryMock()
        {
            _moq =
                new Mock<IDataModelRepository<AggregateDatabaseModel,
                    Lookup>>();

            GetAggregateReturns = RandomAggregateDatabaseModel();

            LookupDeletedReturns = CreateCategoryIndex(3);

            LookupNonDeletedReturns = CreateCategoryIndex(3);

            _moq.Setup(s =>
                    s.GetAggregateAsync(AnyId(), AnyCt()).Result)
                .Returns(GetAggregateReturns);

            _moq.Setup(s =>
                    s.LookupDeletedAsync(AnyCt()).Result)
                .Returns(LookupDeletedReturns);

            _moq.Setup(s =>
                    s.LookupNonDeletedAsync(AnyCt()).Result)
                .Returns(LookupNonDeletedReturns);
        }

        public IDataModelRepository<AggregateDatabaseModel, Lookup> Object =>
            _moq.Object;

        public AggregateDatabaseModel GetAggregateReturns { get; }

        public CategoryIndex<Lookup> LookupNonDeletedReturns { get; }

        public CategoryIndex<Lookup> LookupDeletedReturns { get; }

        public void VerifyInitializeCategory()
        {
            _moq.Verify(s => s.InitializeCategoryIndexes(AnyCt()));
        }

        public void VerifyGetAggregate(Guid key)
        {
            _moq.Verify(s => s.GetAggregateAsync(key, AnyCt()));
        }

        public void VerifyLookupNonDeleted()
        {
            _moq.Verify(s => s.LookupNonDeletedAsync(AnyCt()));
        }

        public void VerifyLookupDeleted()
        {
            _moq.Verify(s => s.LookupDeletedAsync(
                AnyCt()));
        }

        public void VerifyUpsert(AggregateDatabaseModel aggregate)
        {
            _moq.Verify(s => s.UpsertOLD(aggregate, AnyCt()));
        }

        public void VerifyDelete(Guid key)
        {
            _moq.Verify(s => s.DeleteAsync(key, AnyCt()));
        }

        public void VerifyRestore(Guid key)
        {
            _moq.Verify(s => s.RestoreAsync(key, AnyCt()));
        }

        public void VerifyCommit()
        {
            _moq.Verify(s => s.CommitChangesAsync(AnyCt()));
        }

        private readonly
            Mock<IDataModelRepository<AggregateDatabaseModel, Lookup>> _moq;
    }
}