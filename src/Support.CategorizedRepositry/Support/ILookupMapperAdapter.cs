using Jcg.Repositories.Api;

namespace Support.CategorizedRepository.Support;

internal interface ILookupMapperAdapter<TLookupDatabaseModel, TLookup>
    where TLookupDatabaseModel : ILookupDataModel
{
    IEnumerable<TLookup> Map(CategoryIndex<TLookupDatabaseModel> categoryIndex);
}