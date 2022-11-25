using Jcg.CategorizedRepository.Api;

namespace Jcg.CategorizedRepository.CategorizedRepo.Support
{
    internal class LookupMapperAdapter<TLookupDatabaseModel, TLookup>
    : ILookupMapperAdapter<TLookupDatabaseModel, TLookup>
        where TLookupDatabaseModel : ILookupDataModel
    {
        private readonly ILookupMapper<TLookupDatabaseModel, TLookup> _adaptee;

        public LookupMapperAdapter(ILookupMapper<TLookupDatabaseModel, TLookup> adaptee)
        {
            _adaptee = adaptee;
        }
        public IEnumerable<TLookup> Map(CategoryIndex<TLookupDatabaseModel> categoryIndex)
        {
            return categoryIndex.Lookups.Select(l => _adaptee.Map(l)).ToList();
        }
    }
}
