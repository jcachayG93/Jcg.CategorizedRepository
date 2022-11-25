namespace Jcg.CategorizedRepository.Api.Exceptions;

public abstract class RepositoryException : Exception
{
    protected RepositoryException() : base()
    {
    }

    protected RepositoryException(string message) : base(message)
    {
    }
}