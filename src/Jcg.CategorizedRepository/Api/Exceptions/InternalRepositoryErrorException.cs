namespace Jcg.CategorizedRepository.Api.Exceptions
{
    public class InternalRepositoryErrorException : RepositoryException
    {
        public InternalRepositoryErrorException(string error)
            : base(error)
        {
        }
    }
}