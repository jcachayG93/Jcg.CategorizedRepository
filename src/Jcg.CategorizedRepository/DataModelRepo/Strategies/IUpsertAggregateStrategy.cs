using Jcg.CategorizedRepository.Api;
using Jcg.CategorizedRepository.Api.Exceptions;

namespace Jcg.CategorizedRepository.DataModelRepo.Strategies
{
    internal interface IUpsertAggregateStrategy<TAggregateDatabaseModel,
        TLookupDatabaseModel>
        where TAggregateDatabaseModel : class, IAggregateDataModel
    {
        [Obsolete]
// TODO: R200 Remove
        Task UpsertOLD(TAggregateDatabaseModel aggregate,
            CancellationToken cancellationToken);

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