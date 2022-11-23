using Common.Api;
using Support.UnitOfWork.Api.Exceptions;

namespace Support.DataModelRepository.Strategies
{
    internal interface IUpsertAggregateStrategy<TAggregateDatabaseModel,
        TLookupDatabaseModel>
        where TAggregateDatabaseModel : class, IAggregateDataModel
        where TLookupDatabaseModel : ILookupDataModel
    {
        /// <summary>
        ///     Upsers the aggregate.
        /// </summary>
        /// <param name="aggregate">The aggregate</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="CategoryIndexIsUninitializedException">
        ///     When the CategoryIndex
        ///     is not found
        /// </exception>
        Task UpsertAsync(TAggregateDatabaseModel aggregate,
            CancellationToken cancellationToken);
    }
}