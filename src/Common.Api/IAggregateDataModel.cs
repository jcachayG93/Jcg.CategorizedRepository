namespace Jcg.Repositories.Api;

/// <summary>
/// The model that represents one complete aggregate. This model will be stored in the database so it must be compatible with it.
/// </summary>
public interface IAggregateDataModel
{
    /// <summary>
    /// This is the key that will be associated with this entity in the database.
    /// </summary>
    string Key { get; set; }
}