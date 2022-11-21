namespace Support.UnitOfWork.Cache;

/// <summary>
/// Represents an item that was upserted in the local cache. This can be an Aggregate o a CategoryIndex
/// </summary>
/// <typeparam name="TData">The payload</typeparam>
/// <param name="key">The key, if the item is a category index then this is the category key.</param>
/// <param name="ETag">The ETag associated with the payload. Blank for a payload that was created locally, otherwise for one that was retrieved from the database</param>
/// <param name="payLoad">The payload</param>
internal record UpsertedItem<TData>(string key, string ETag, TData payLoad);