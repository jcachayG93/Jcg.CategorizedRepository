using Common.Api;
using Support.UnitOfWork.Api;

namespace Support.CategorizedRepository.Support;

internal interface ILookupMapperAdapter<TLookupDatabaseModel, TLookup>
    where TLookupDatabaseModel : IRepositoryLookup
{
    IEnumerable<TLookup> Map(CategoryIndex<TLookupDatabaseModel> categoryIndex);
}