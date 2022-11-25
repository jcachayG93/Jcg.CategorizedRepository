namespace Jcg.DataAccessRepositories
{
    /// <summary>
    ///     The basic data needed for the Lookup Data Model which is the model
    ///     that represents the lookup that will be stored in the database.
    /// </summary>
    public interface ILookupDataModel
    {
        string Key { get; set; }

        bool IsDeleted { get; set; }

        string DeletedTimeStamp { get; set; }
    }
}