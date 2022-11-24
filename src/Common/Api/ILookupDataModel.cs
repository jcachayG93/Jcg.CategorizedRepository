namespace Common.Api
{
    public interface ILookupDataModel
    {
        string Key { get; set; }

        bool IsDeleted { get; set; }

        string DeletedTimeStamp { get; set; }
    }
}