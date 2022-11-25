using Jcg.CategorizedRepository.Api.Exceptions;

namespace Jcg.CategorizedRepository.DataModelRepo.Strategies
{
    internal interface ICommitStrategy
    {
        /// <summary>
        ///     Commits all the changes. This operation can be called only once for the lifetime of this UnitOfWork
        /// </summary>
        /// <returns></returns>
        /// <exception cref="UnitOfWorkWasAlreadyCommittedException">
        ///     Thrown when called more than once. This unit of work can be committed up to one time only.
        /// </exception>
        Task CommitChangesAsync(
            CancellationToken cancellationToken);
    }
}