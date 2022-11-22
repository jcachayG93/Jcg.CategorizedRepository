namespace Common.Api
{
    [Obsolete]
// TODO: R200 Remove
    public interface IRepositoryKey
    {
        string Key { get; set; }
    }

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