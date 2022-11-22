namespace Testing.Common.Doubles;

public class DatabaseException : Exception
{
    public DatabaseException(string error) : base(error)
    {
    }
}