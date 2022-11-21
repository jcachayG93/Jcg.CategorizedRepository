using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Support.UnitOfWork.Api;

namespace Support.UnitOfWork.Cache.Imp
{
    internal class AggregatesCache<TAggregatesDatabaseModel>
    where TAggregatesDatabaseModel : class
    {
        /// <summary>
        /// Checks if an Add operation has been done for the specified key
        /// </summary>
        public bool HasKey(string key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the cached aggregate for the key, which could be null.
        /// Throws exception if the key has not been read.
        /// </summary>
        TAggregatesDatabaseModel? Get(string key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds the payload for the specified key to the cache. The payload could be
        /// null but the HasKey method will return true anyway.
        /// </summary>
        void Add(string key, IETagDto<TAggregatesDatabaseModel> payload)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Upserts the data for the key: Replaces the payload in the local cache so the next time
        /// Get is called, this will be returned. Adds the payload to the Upserted Items collection.
        /// </summary>
        void Upsert(string key, TAggregatesDatabaseModel data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The items that were passed to the Upsert method. If that was an insert operation,
        /// the ETag will be null, if it was that was added and then updated, the ETag will
        /// be the the value originally passed to the Add method
        /// </summary>
        IEnumerable<UpsertedItem<TAggregatesDatabaseModel>> UpsertedItems()
        {
            throw new NotImplementedException();
        }
    }
}
