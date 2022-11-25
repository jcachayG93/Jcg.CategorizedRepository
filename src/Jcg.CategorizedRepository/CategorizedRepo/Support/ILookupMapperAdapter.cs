using Jcg.CategorizedRepository.Api;

namespace Jcg.CategorizedRepository.CategorizedRepo.Support;

internal interface ILookupMapperAdapter<TLookupDatabaseModel, TLookup>
{
    IEnumerable<TLookup> Map(CategoryIndex<TLookupDatabaseModel> categoryIndex);
}