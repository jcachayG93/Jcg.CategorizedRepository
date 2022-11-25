namespace Jcg.CategorizedRepository.UoW.InternalExceptions
{
    public class CacheException : Exception
    {
        public CacheException(string error)
            : base(error)
        {
        }
    }
}