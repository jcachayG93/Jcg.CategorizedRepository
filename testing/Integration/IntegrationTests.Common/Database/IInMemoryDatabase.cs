using Support.UnitOfWork.Api;

namespace IntegrationTests.Common.Database
{
    public interface IInMemoryDatabase
    {
        IETagDto<T>? GetAggregate<T>(string key);

        void UpsertAndCommit(IEnumerable<UpsertOperation> operations);
    }
}