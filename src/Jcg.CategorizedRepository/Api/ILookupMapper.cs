namespace Jcg.CategorizedRepository.Api;

[Obsolete]
// TODO: R200 Remove
public interface ILookupMapper<TLookupDatabaseModel, TLookup>
{
    TLookup Map(TLookupDatabaseModel databaseModel);
}