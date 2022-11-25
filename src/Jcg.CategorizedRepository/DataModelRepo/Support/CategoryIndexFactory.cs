using Jcg.CategorizedRepository.Api;

namespace Jcg.CategorizedRepository.DataModelRepo.Support
{
    internal class CategoryIndexFactory<TLookupDatabaseModel>
    {
        public virtual CategoryIndex<TLookupDatabaseModel> Create()
        {
            return new();
        }
    }
}