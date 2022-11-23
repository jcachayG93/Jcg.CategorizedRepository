using IntegrationTests.Common.Types;
using Support.UnitOfWork.Api;

namespace IntegrationTests.Common.Database
{
    public interface IInMemoryDatabase
    {
        IETagDto<T>? GetAggregate<T>(string key)
            where T : IClone;

        void UpsertAndCommit(IEnumerable<UpsertOperation> operations);
    }
}