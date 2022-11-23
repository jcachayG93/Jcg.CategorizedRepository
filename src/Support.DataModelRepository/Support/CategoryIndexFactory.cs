using Common.Api;
using Support.UnitOfWork.Api;

namespace Support.DataModelRepository.Support
{
    internal class CategoryIndexFactory<TLookupDatabaseModel>
        where TLookupDatabaseModel : IRepositoryLookup
    {
        public virtual CategoryIndex<TLookupDatabaseModel> Create()
        {
            return new();
        }
    }
}