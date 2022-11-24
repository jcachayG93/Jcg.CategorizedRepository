namespace IntegrationTests.Common.Database;

public class InMemoryDatabase : IInMemoryDatabase
{
    /// <inheritdoc />
    public void UpsertAndCommit(IEnumerable<UpsertOperation> operations)
    {
        lock (_lockObject)
        {
            operations = operations.ToList();

            var etag = operations.First().ETag;

            AssertItemsWithBlankETagDoNotExistInDatabase(operations);

            AssertNonBlankETagsMatchDataETags(operations);

            operations = EvolveETags(operations);

            ApplyChanges(EvolveETags(operations));
        }
    }

    /// <inheritdoc />
    public DataRecord? Get(string key)

    {
        if (_data.TryGetValue(key, out var data))
        {
            return data;
        }

        return null;
    }

    private void AssertItemsWithBlankETagDoNotExistInDatabase(
        IEnumerable<UpsertOperation> operations)
    {
        foreach (var op in operations.Where(o =>
                     string.IsNullOrWhiteSpace(o.ETag)).ToList())
        {
            if (_data.ContainsKey(op.Key))
            {
                throw new OptimisticConcurrencyException(
                    "At least one operation ETag is blank but data for that key already exists in the database");
            }
        }
    }

    private void AssertNonBlankETagsMatchDataETags(
        IEnumerable<UpsertOperation> operations)
    {
        foreach (var op in operations
                     .Where(o => !string.IsNullOrWhiteSpace(o.ETag))
                     .ToList())
        {
            var item = _data[op.Key];

            if (item.ETag != op.ETag)
            {
                throw new OptimisticConcurrencyException("ETag mismatch");
            }
        }
    }

    private IEnumerable<UpsertOperation> EvolveETags(
        IEnumerable<UpsertOperation> operations)
    {
        return operations.Select(o =>
                o with
                {
                    ETag = Guid.NewGuid().ToString()
                })
            .ToList();
    }

    private void ApplyChanges(IEnumerable<UpsertOperation> operations)
    {
        foreach (var op in operations.ToList())
        {
            var dr = new DataRecord(op.ETag, op.Payload);

            if (_data.ContainsKey(op.Key))
            {
                _data[op.Key] = dr;
            }
            else
            {
                _data.Add(op.Key, dr);
            }
        }
    }

    private static readonly object _lockObject = new();

    private readonly Dictionary<string, DataRecord> _data = new();
}