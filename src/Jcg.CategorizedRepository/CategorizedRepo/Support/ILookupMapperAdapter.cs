using Jcg.CategorizedRepository.Api;

namespace Jcg.CategorizedRepository.CategorizedRepo.Support;

internal interface ILookupMapperAdapter<TLookupDatabaseModel, TLookup>
    where TLookupDatabaseModel : ILookupDataModel
{
    IEnumerable<TLookup> Map(CategoryIndex<TLookupDatabaseModel> categoryIndex);
}