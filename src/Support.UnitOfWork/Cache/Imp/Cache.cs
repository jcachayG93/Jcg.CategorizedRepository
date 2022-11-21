using Support.UnitOfWork.Api;

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
            UpsertedItems { get; }

        /// <summary>
        ///     Checks if an Add operation has been done for the specified key
        /// </summary>
        public virtual bool HasKey(string key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Gets the cached data for the key, which could be null.
        ///     Throws exception if the key has not been read.
        /// </summary>
        public virtual TData? Get(string key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Adds the payload for the specified key to the cache. The data could be
        ///     null but the HasKey method will return true anyway.
        /// </summary>
        public virtual void Add(string key,
            IETagDto<TData> data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Upserts the data for the key: Replaces the payload in the local cache so the next time
        ///     Get is called, this will be returned. Adds the payload to the Upserted Items collection.
        /// </summary>
        public virtual void Upsert(string key, TData payload)
        {
            throw new NotImplementedException();
        }
    }
}