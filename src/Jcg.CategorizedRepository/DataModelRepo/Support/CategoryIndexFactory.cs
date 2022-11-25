using Jcg.CategorizedRepository.Api;

namespace Jcg.CategorizedRepository.DataModelRepo.Support
{
    internal class CategoryIndexFactory<TLookupDatabaseModel>
        where TLookupDatabaseModel : ILookupDataModel
    {
        public virtual CategoryIndex<TLookupDatabaseModel> Create()
        {
            return new();
        }
    }
}