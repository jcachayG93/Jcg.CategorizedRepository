using Common.Api;
using Support.UnitOfWork.Api;

namespace Support.CategorizedRepository.Support;

internal interface ILookupMapperAdapter<TLookupDatabaseModel, Lookup>
    where TLookupDatabaseModel : IRepositoryLookup
{
    IEnumerable<Lookup> Map(CategoryIndex<TLookupDatabaseModel> lookups);
}