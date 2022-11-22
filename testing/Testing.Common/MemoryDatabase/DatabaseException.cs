namespace Testing.Common.Doubles;

internal class DatabaseException : Exception
{
    public DatabaseException(string error) : base(error)
    {
    }
}