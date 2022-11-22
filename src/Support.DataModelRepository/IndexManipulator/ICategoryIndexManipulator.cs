using Common.Api;
using Support.UnitOfWork.Api;

namespace Support.DataModelRepository.IndexManipulator
{
    /// <summary>
    ///     Manipulates the CategoryIndex
    /// </summary>
    /// <typeparam name="TAggregateDatabaseModel"></typeparam>
    /// <typeparam name="TLookupDatabaseModel"></typeparam>
    public interface ICategoryIndexManipulator
        <TAggregateDatabaseModel, TLookupDatabaseModel>
        where TAggregateDatabaseModel : class
        where TLookupDatabaseModel : class, IRepositoryLookup
    {
        /// <summary>
        ///     If a lookup exist for the key, replaces it, else inserts a new one. The lookup will be mapped from the aggregate
        /// </summary>
        /// <param name="nonDeletedCategoryIndex"></param>
        /// <param name="key"></param>
        /// <param name="aggregate"></param>
        void Upsert(CategoryIndex<TLookupDatabaseModel> nonDeletedCategoryIndex,
            string key,
            TAggregateDatabaseModel aggregate);

        void Delete(CategoryIndex<TLookupDatabaseModel> nonDeletedCategoryIndex,
            CategoryIndex<TLookupDatabaseModel> deletedCategoryIndex,
            string key);

        void Restore(
            CategoryIndex<TLookupDatabaseModel> nonDeletedCategoryIndex,
            CategoryIndex<TLookupDatabaseModel> deletedCategoryIndex,
            string key);
    }
}