namespace Testing.CommonV2.MemoryDatabase;

public class DatabaseException : Exception
{
    public DatabaseException(string error) : base(error)
    {
    }
}