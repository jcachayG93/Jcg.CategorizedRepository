namespace Jcg.DataAccessRepositories
{
    /// <summary>
    ///     A Dto that wraps a Payload with an associated ETag value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IETagDto<out T>
    {
        /// <summary>
        ///     The ETag value for the associated payload
        /// </summary>
        public string Etag { get; }

        /// <summary>
        ///     The payload
        /// </summary>
        public T Payload { get; }
    }
}