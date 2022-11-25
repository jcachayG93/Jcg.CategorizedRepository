using Jcg.CategorizedRepository.Api;

namespace Jcg.CategorizedRepository.DataModelRepo.Support.IndexManipulator
{
    /// <summary>
    ///     Perform operations in category indexes
    /// </summary>
    internal interface ICategoryIndexManipulator
        <TAggregateDatabaseModel, TLookupDatabaseModel>
        where TAggregateDatabaseModel : class, IAggregateDataModel
        where TLookupDatabaseModel : ILookupDataModel
    {
        /// <summary>
        ///     Upserts: If data for the key exists, it replaces it. IF it doesnt, it inserts it.
        /// </summary>
        void Upsert(CategoryIndex<TLookupDatabaseModel> nonDeletedCategoryIndex,
            TAggregateDatabaseModel aggregate);

        /// <summary>
        ///     Moves a lookup from the non-deleted to the deleted category indexes, setting its deleted value to true and its
        ///     time-stamp
        /// </summary>
        /// <param name="nonDeletedCategoryIndex">The category index that contains non-deleted items</param>
        /// <param name="deletedCategoryIndex">The category index that contains deleted items</param>
        /// <param name="key">The key</param>
        void Delete(
            CategoryIndex<TLookupDatabaseModel> nonDeletedCategoryIndex,
            CategoryIndex<TLookupDatabaseModel> deletedCategoryIndex,
            string key,
            DateTime timeStamp);

        /// <summary>
        ///     Moves a lookup from the deleted to the non-deleted category indexes, setting the lookup delete timeStamp to default
        ///     and IsDeleted to false
        /// </summary>
        /// <param name="nonDeletedCategoryIndex">The category index that contains non-deleted items</param>
        /// <param name="deletedCategoryIndex">The category index that contains deleted items</param>
        /// <param name="key">The key</param>
        void Restore(
            CategoryIndex<TLookupDatabaseModel> nonDeletedCategoryIndex,
            CategoryIndex<TLookupDatabaseModel> deletedCategoryIndex,
            string key);
    }
}