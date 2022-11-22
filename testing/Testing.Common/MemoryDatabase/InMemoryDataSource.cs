namespace Testing.Common.Doubles;

public class InMemoryDataSource
{
    public AggregateETag? GetAggregate(string key)
    {
        if (!_aggregates.ContainsKey(key))
        {
            return null;
        }

        return _aggregates[key].Clone();
    }

    public CategoryIndexETag? GetCategoryIndex(string key)
    {
        if (!_categoryIndexes.ContainsKey(key))
        {
            return null;
        }

        return _categoryIndexes[key].Clone();
    }

    public void Upsert(Dictionary<string, AggregateETag> aggregates,
        Dictionary<string, CategoryIndexETag> categoryIndexes)
    {
        AssignETagIfBlank(aggregates, categoryIndexes);


        AssertETagsMatch(aggregates, categoryIndexes);

        SaveAggregates(aggregates);

        SaveCategoryIndexes(categoryIndexes);
    }

    private void AssignETagIfBlank(Dictionary<string, AggregateETag> aggregates,
        Dictionary<string, CategoryIndexETag> categoryIndexes)
    {
        foreach (var aggregate in aggregates)
        {
            if (string.IsNullOrWhiteSpace(aggregate.Value.Etag))
            {
                aggregates[aggregate.Key] = aggregate.Value.CloneWithNewETag();
            }
        }

        foreach (var index in categoryIndexes)
        {
            if (string.IsNullOrWhiteSpace(index.Value.Etag))
            {
                categoryIndexes[index.Key] = index.Value.CloneWithNewETag();
            }
        }
    }

    private void SaveCategoryIndexes(
        Dictionary<string, CategoryIndexETag> categoryIndexes)
    {
        var updatedValues = categoryIndexes.ToDictionary(i => i.Key,
            i => i.Value.CloneWithNewETag());

        foreach (var index in updatedValues)
        {
            if (_categoryIndexes.ContainsKey(index.Key))
            {
                _categoryIndexes[index.Key] = index.Value;
            }
            else
            {
                _categoryIndexes.Add(index.Key, index.Value);
            }
        }
    }

    private void SaveAggregates(
        Dictionary<string, AggregateETag> aggregates)
    {
        var updatedAggregates = aggregates.ToDictionary(i => i.Key,
            i => i.Value.CloneWithNewETag());

        foreach (var aggregate in updatedAggregates)
        {
            if (_aggregates.ContainsKey(aggregate.Key))
            {
                _aggregates[aggregate.Key] = aggregate.Value;
            }
            else
            {
                _aggregates.Add(aggregate.Key, aggregate.Value);
            }
        }
    }


    private void AssertETagsMatch(
        Dictionary<string, AggregateETag> aggregates,
        Dictionary<string, CategoryIndexETag> categoryIndexes)
    {
        foreach (var aggregate in aggregates)
        {
            if (_aggregates.ContainsKey(aggregate.Key))
            {
                if (_aggregates[aggregate.Key].Etag != aggregate.Value.Etag)
                {
                    throw new DatabaseException("Aggregate Etag mistmatch");
                }
            }
        }

        foreach (var index in categoryIndexes)
        {
            if (_categoryIndexes.ContainsKey(index.Key))
            {
                var item = _categoryIndexes[index.Key];


                if (_categoryIndexes[index.Key].Etag != index.Value.Etag)
                {
                    throw new DatabaseException(
                        "CategoryIndex Etag Mismatch");
                }
            }
        }
    }

    private readonly Dictionary<string, AggregateETag> _aggregates = new();

    private readonly Dictionary<string, CategoryIndexETag>
        _categoryIndexes = new();
}