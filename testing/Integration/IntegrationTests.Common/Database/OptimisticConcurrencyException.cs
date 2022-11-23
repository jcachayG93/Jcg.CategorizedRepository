namespace IntegrationTests.Common.Database;

public class OptimisticConcurrencyException : Exception
{
    public OptimisticConcurrencyException(string error) : base(error)
    {
    }
}