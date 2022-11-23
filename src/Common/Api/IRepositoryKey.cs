namespace Common.Api
{
    public interface IRepositoryLookup
    {
        string Key { get; set; }

        bool IsDeleted { get; set; }

        string DeletedTimeStamp { get; set; }
    }

    public interface IAggregateDataModel
    {
        string Key { get; set; }
    }
}