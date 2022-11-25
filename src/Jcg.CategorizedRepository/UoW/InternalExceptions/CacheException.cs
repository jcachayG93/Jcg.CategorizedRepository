namespace Support.UnitOfWork.InternalExceptions
{
    public class CacheException : Exception
    {
        public CacheException(string error)
            : base(error)
        {
        }
    }
}