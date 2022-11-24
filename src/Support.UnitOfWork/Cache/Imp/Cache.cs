using Common.Api;
using Support.UnitOfWork.InternalExceptions;

namespace Support.UnitOfWork.Cache.Imp
{
    internal class Cache<TData>
        where TData : class

    {
        /// <summary>
        ///     The items that were passed to the Upsert method. If that was an insert operation,
        ///     the ETag will be null, if it was that was added and then updated, the ETag will
        ///     be the the value originally passed to the Add method
        /// </summary>
        public virtual IEnumerable<UpsertedItem<TData>>
            UpsertedItems
        {
            get
            {
                var items = _data.Where(i =>
                        i.Value.WasUpserted)
                    .Select(i =>
                        // I can use the ! because to mark an item as Upserted it must receive a non null payload. See CacheItem definition bellow.
                        new UpsertedItem<TData>(i.Key, i.Value.ETag,
                            i.Value.PayLoad!))
                    .ToList();

                return items;
            }
        }


        /// <summary>
        ///     Checks if an Add operation has been done for the specified key
        /// </summary>
        public virtual bool HasKey(string key)
        {
            return _data.ContainsKey(key);
        }

        /// <summary>
        ///     Gets the cached data for the key, which could be null.
        ///     Throws exception if the key has not been read.
        /// </summary>
        public virtual TData? Get(string key)
        {
            if (!HasKey(key))
            {
                throw new CacheException("No data for key");
            }

            return _data[key].PayLoad;
        }

        /// <summary>
        ///     Adds the payload for the specified key to the cache. The data could be
        ///     null but the HasKey method will return true anyway.
        ///     Throws an exception if key is already in the cache
        /// </summary>
        public virtual void Add(string key,
            IETagDto<TData>? data)
        {
            if (HasKey(key))
            {
                throw new CacheException("Data for key already exists");
            }

            var eTag = data?.Etag ?? "";

            var payload = data?.Payload ?? null;

            _data.Add(key, new(eTag, payload));
        }

        /// <summary>
        ///     Upserts the data for the key: Replaces the payload in the local cache so the next time
        ///     Get is called, this will be returned. Adds the payload to the Upserted Items collection.
        /// </summary>
        public virtual void Upsert(string key, TData payload)
        {
            if (!HasKey(key))
            {
                Add(key, null);
            }

            var item = _data[key];

            item.Upsert(payload);
        }

        private readonly Dictionary<string, CacheItem> _data = new();

        private class CacheItem
        {
            public CacheItem(string eTag, TData? payLoad)
            {
                ETag = eTag;
                PayLoad = payLoad;
                WasUpserted = false;
            }

            public string ETag { get; }

            public TData? PayLoad { get; private set; }

            public bool WasUpserted { get; private set; }

            public void Upsert(TData updatedPayload)
            {
                PayLoad = updatedPayload;
                WasUpserted = true;
            }
        }
    }
}