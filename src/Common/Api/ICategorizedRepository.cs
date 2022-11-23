using Common.Api.Exceptions;

namespace Common.Api
{
    public interface ICategorizedRepository
        <TAggregate, TLookup>
    {
        /// <summary>
        ///     Initializes the category index.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="CategoryIndexIsAlreadyInitializedException">Thrown if the category already exists</exception>
        Task InitializeCategoryIndexAsync(CancellationToken cancellationToken);

        // TODO: Continue here...
        Task<TAggregate?> GetAggregateAsync(RepositoryIdentity key,
            CancellationToken cancellationToken);

        Task UpsertAsync(RepositoryIdentity key, TAggregate aggregate,
            CancellationToken cancellationToken);

        Task<IEnumerable<TLookup>> LookupAsync(
            CancellationToken cancellationToken);

        Task<IEnumerable<TLookup>> LookupDeletedAsync(
            CancellationToken cancellationToken);

        Task DeleteAsync(RepositoryIdentity key,
            CancellationToken cancellationToken);

        Task RestoreAsync(RepositoryIdentity key,
            CancellationToken cancellationToken);

        Task CommitChangesAsync(CancellationToken cancellationToken);
    }

    public record RepositoryIdentity
    {
        public RepositoryIdentity(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new RepositoryIdentityValueCantBeEmptyException();
            }

            Value = value;
        }

        public Guid Value { get; }
    }
}