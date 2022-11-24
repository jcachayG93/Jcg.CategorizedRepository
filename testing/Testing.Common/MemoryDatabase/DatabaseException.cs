namespace Testing.Common.MemoryDatabase;

public class DatabaseException : Exception
{
    public DatabaseException(string error) : base(error)
    {
    }
}