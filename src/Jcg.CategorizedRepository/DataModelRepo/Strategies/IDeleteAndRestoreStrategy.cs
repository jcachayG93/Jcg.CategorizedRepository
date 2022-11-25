using Jcg.CategorizedRepository.Api;
using Jcg.CategorizedRepository.Api.Exceptions;

namespace Jcg.CategorizedRepository.DataModelRepo.Strategies
{
    internal interface IDeleteAndRestoreStrategy
        <TAggregateDatabaseModel, TLookupDatabaseModel>
        where TAggregateDatabaseModel : class, IAggregateDataModel
        where TLookupDatabaseModel : IRepositoryLookup
    {
        /// <summary>
        ///     Moves the lookup from the Non-Deleted to the Deleted category index
        /// </summary>
        /// <param name="key">The aggregate key</param>
        /// <exception cref="CategoryIndexIsUninitializedException">
        ///     When the CategoryIndex
        ///     is not found
        /// </exception>
        Task DeleteAsync(Guid key, CancellationToken cancellationToken);

        /// <summary>
        ///     Moves the lookup from the Deleted to the Non-Deleted category index
        /// </summary>
        /// <param name="key">The aggregate key</param>
        /// <exception cref="CategoryIndexIsUninitializedException">
        ///     When the CategoryIndex
        ///     is not found
        /// </exception>
        Task RestoreAsync(Guid key, CancellationToken cancellationToken);
    }
}