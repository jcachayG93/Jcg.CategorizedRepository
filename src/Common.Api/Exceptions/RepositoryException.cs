namespace Jcg.Repositories.Api.Exceptions;

public abstract class RepositoryException : Exception
{
    protected RepositoryException() : base()
    {
    }

    protected RepositoryException(string message) : base(message)
    {
    }
}