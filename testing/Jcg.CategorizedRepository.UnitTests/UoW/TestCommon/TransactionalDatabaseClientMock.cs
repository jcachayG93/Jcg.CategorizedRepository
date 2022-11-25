using Jcg.CategorizedRepository.Api;
using Jcg.CategorizedRepository.Api.DatabaseClient;
using Moq;
using Testing.CommonV2.Types;

namespace Jcg.CategorizedRepository.UnitTests.UoW.TestCommon
{
    internal class TransactionalDatabaseClientMock
    {
        public TransactionalDatabaseClientMock()
        {
            _moq = new();

            GetAggregateReturns = RandomAggregateETagDto();

            GetCategoryIndexReturns = RandomCategoryIndexETagDto();

            SetupGetAggregate(GetAggregateReturns);

            SetupGetCategoryIndex(GetCategoryIndexReturns);
        }

        public IETagDto<AggregateDatabaseModel> GetAggregateReturns { get; }

        public IETagDto<CategoryIndex<Lookup>>
            GetCategoryIndexReturns { get; }

        public ITransactionalDatabaseClient<AggregateDatabaseModel,
            Lookup> Object => _moq.Object;

        private void SetupGetAggregate(
            IETagDto<AggregateDatabaseModel> returns)
        {
            _moq.Setup(s =>
                    s.GetAggregateAsync(AnyString(), AnyCt()).Result)
                .Returns(returns);
        }

        private void SetupGetCategoryIndex(
            IETagDto<CategoryIndex<Lookup>> returns)
        {
            _moq.Setup(s =>
                    s.GetCategoryIndex(AnyString(), AnyCt()).Result)
                .Returns(returns);
        }

        public void SetupGetCategoryIndexReturnsNull()
        {
            SetupGetCategoryIndex(null);
        }

        public void VerifyGetAggregate(string key)
        {
            _moq.Verify(s =>
                s.GetAggregateAsync(key, AnyCt()));
        }

        public void VerifyNoOtherCalls()
        {
            _moq.VerifyNoOtherCalls();
        }


        public void VerifyUpsertAggregate(string key, string eTag,
            AggregateDatabaseModel aggregate)
        {
            _moq.Verify(s =>
                s.UpsertAggregateAsync(key, eTag, aggregate, AnyCt()));
        }

        public void VerifyGetCategoryIndex(string categoryKey)
        {
            _moq.Verify(s => s.GetCategoryIndex(categoryKey, AnyCt()));
        }

        public void VerifyUpsertCategoryIndex(string key, string eTag,
            CategoryIndex<Lookup> categoryIndex)
        {
            _moq.Verify(s =>
                s.UpsertCategoryIndex(key, eTag, categoryIndex, AnyCt()));
        }

        public void VerifyCommitTransaction()
        {
            _moq.Verify(s => s.CommitTransactionAsync(AnyCt()));
        }

        private readonly Mock<ITransactionalDatabaseClient<
            AggregateDatabaseModel, Lookup>> _moq;
    }
}