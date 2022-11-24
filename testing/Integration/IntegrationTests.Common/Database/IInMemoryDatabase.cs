namespace IntegrationTests.Common.Database
{
    public interface IInMemoryDatabase
    {
        DataRecord? Get(string key);


        void UpsertAndCommit(IEnumerable<UpsertOperation> operations);
    }
}