using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Support.UnitOfWork.Api
{
    public interface ITransactionalDatabaseClient<
        TAggregateDatabaseModel,
        TLookupDatabaseModel>
        where TAggregateDatabaseModel : class
        where TLookupDatabaseModel : class
    {
        Task<IETagDto<TAggregateDatabaseModel>?> GetAggregateAsync(string key,
            CancellationToken cancellationToken);

        Task UpsertAggregateAsync(string key, string eTag,
            TAggregateDatabaseModel aggregate,
            CancellationToken cancellationToken);

        Task<IETagDto<CategoryIndex<TLookupDatabaseModel>>?> GetCategoryIndex(
            string key,
            CancellationToken cancellationToken);

        Task UpsertCategoryIndex(string key,
            string eTag,
            CategoryIndex<TLookupDatabaseModel> categoryIndex,
            CancellationToken cancellationToken);

        Task CommitTransactionAsync(CancellationToken cancellationToken);
    }
}
