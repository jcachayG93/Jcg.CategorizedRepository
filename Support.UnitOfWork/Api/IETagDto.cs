using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Support.UnitOfWork.Api
{
    /// <summary>
    /// A Dto that wraps a Payload with an associated ETag value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IETagDto<out T> where T : class
    {
        /// <summary>
        /// The ETag value for the associated payload
        /// </summary>
        public string Etag { get; }

        /// <summary>
        /// The payload
        /// </summary>
        public T Payload { get; }
    }
}
