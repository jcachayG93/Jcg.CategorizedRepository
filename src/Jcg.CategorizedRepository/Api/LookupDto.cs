namespace Jcg.CategorizedRepository.Api;

public class LookupDto<TLookupDatabaseModel>
{
    public string Key { get; set; }

    public bool IsDeleted { get; set; }

    public string DeletedTimeStamp { get; set; }

    public TLookupDatabaseModel PayLoad { get; set; }
}