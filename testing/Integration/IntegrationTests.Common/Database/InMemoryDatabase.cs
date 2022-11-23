using IntegrationTests.Common.Types;
using Support.UnitOfWork.Api;

namespace IntegrationTests.Common.Database;

public class InMemoryDatabase : IInMemoryDatabase
{
    /// <inheritdoc />
    public IETagDto<T>? GetAggregate<T>(string key)
        where T : IClone
    {
        if (_data.TryGetValue(key, out var data))
        {
            var payload = (T)data.Payload;

            return new ETagDtoImp<T>(data.ETag, (T)payload.Clone());
        }

        return null;
    }

    /// <inheritdoc />
    public void UpsertAndCommit(IEnumerable<UpsertOperation> operations)
    {
        lock (_lockObject)
        {
            operations = operations.ToList();

            AssertItemsWithBlankETagDoNotExistInDatabase(operations);

            AssertNonBlankETagsMatchDataETags(operations);

            ApplyChanges(EvolveETags(operations));
        }
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
            var dr = new DataRecord(op.ETag, (IClone)op.Payload.Clone());

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


    private record DataRecord(string ETag, IClone Payload);
}

internal class ETagDtoImp<T> : IETagDto<T>
{
    public ETagDtoImp(string etag, T payload)
    {
        Etag = etag;
        Payload = payload;
    }

    /// <inheritdoc />
    public string Etag { get; }

    /// <inheritdoc />
    public T Payload { get; }
}