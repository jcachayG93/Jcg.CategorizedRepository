using Jcg.CategorizedRepository.Api;
using Jcg.CategorizedRepository.Api.Exceptions;

namespace Jcg.CategorizedRepository.DataModelRepo.Strategies
{
    internal interface IUpsertAggregateStrategy<TAggregateDatabaseModel,
        TLookupDatabaseModel>
        where TAggregateDatabaseModel : class, IAggregateDataModel
    {
        /// <summary>
        ///     Upsers the aggregate.
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="aggregate">The aggregate</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="CategoryIndexIsUninitializedException">
        ///     When the CategoryIndex
        ///     is not found
        /// </exception>
        Task UpsertAsync(
            string key,
            TAggregateDatabaseModel aggregate,
            CancellationToken cancellationToken);
    }
}