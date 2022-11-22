using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Support.UnitOfWork;
using Support.UnitOfWork.Api;
using Testing.Common.Types;

namespace Common.UnitTests.TestCommon
{

    internal class UnitOfWorkImpMock
    {
        public UnitOfWorkImpMock()
        {
            _moq = new Mock<IUnitOfWorkImp<AggregateDatabaseModel, LookupDatabaseModel>>();

            GetNonDeletedItemsCategoryIndexReturns = RandomCategoryIndex();

            _moq.Setup(s => s.GetNonDeletedItemsCategoryIndex(AnyCt()).Result)
                .Returns(GetNonDeletedItemsCategoryIndexReturns);

            GetDeletedItemsCategoryIndexReturns = RandomCategoryIndex();

            _moq.Setup(s => s.GetDeletedItemsCategoryIndex(AnyCt()).Result)
                .Returns(GetDeletedItemsCategoryIndexReturns);

            GetAggregateReturns = RandomAggregateDatabaseModel();

            _moq.Setup(s=>
                s.GetAggregateAsync(AnyString(), AnyCt()).Result)
                .Returns(GetAggregateReturns);
        }

        private readonly Mock<IUnitOfWorkImp<AggregateDatabaseModel, LookupDatabaseModel>> _moq;

        public IUnitOfWorkImp<AggregateDatabaseModel, LookupDatabaseModel> Object => _moq.Object;

        public void VerifyGetNonDeletedItemsCategoryIndex(CancellationToken cancellationToken)
        {
            _moq.Verify(s=>
                s.GetNonDeletedItemsCategoryIndex(cancellationToken));
        }

        public CategoryIndex<LookupDatabaseModel> GetNonDeletedItemsCategoryIndexReturns { get; }


        public void VerifyGetDeletedItemsCategoryIndex(CancellationToken cancellationToken)
        {
            _moq.Verify(s =>
                s.GetDeletedItemsCategoryIndex(cancellationToken));
        }

        public CategoryIndex<LookupDatabaseModel> GetDeletedItemsCategoryIndexReturns { get; }

        public void VerifyUpsertDeletedItemsCategoryIndex(
            CategoryIndex<LookupDatabaseModel> deletedItemsCategoryIndex,
            CancellationToken cancellationToken)
        {
            _moq.Verify(s=>
                s.UpsertDeletedItemsCategoryIndex(deletedItemsCategoryIndex, cancellationToken));
        }

        public void VerifyUpsertNonDeletedItemsCategoryIndex(
            CategoryIndex<LookupDatabaseModel> nonDeletedItemsCategoryIndex,
            CancellationToken cancellationToken)
        {
            _moq.Verify(s =>
                s.UpsertNonDeletedItemsCategoryIndex(nonDeletedItemsCategoryIndex, cancellationToken));
        }

        public void VerifyGetAggregate(string key, CancellationToken cancellationToken)
        {
            _moq.Verify(s=>
                s.GetAggregateAsync(key, cancellationToken));
        }

        public AggregateDatabaseModel GetAggregateReturns { get; }

        public void VerifyUpsertAggregate(string key,
            AggregateDatabaseModel aggregate, CancellationToken cancellationToken)
        {
            _moq.Verify(s=>s.UpsertAggregateAsync(key, aggregate, cancellationToken));
        }

        public void VerifyCommitChanges(CancellationToken cancellationToken)
        {
            _moq.Verify(s=>s.CommitChangesAsync(
                cancellationToken));
        }
    }
}
