using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Support.UnitOfWork.Api
{
    /// <summary>
    /// Represents an index table to lookup items items belonging to a category
    /// </summary>
    /// <typeparam name="TLookupDatabaseModel"></typeparam>
    public class CategoryIndex<TLookupDatabaseModel>
    {
        public IEnumerable<TLookupDatabaseModel> Lookups { get; set; }
        = Array.Empty<TLookupDatabaseModel>();
    }
}
