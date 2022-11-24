using Jcg.Repositories.Api;

namespace Support.DataModelRepository.Support
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