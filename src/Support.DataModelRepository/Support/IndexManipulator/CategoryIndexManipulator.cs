using Common.Api;
using Common.Api.Api;
using Common.Api.Exceptions;
using Support.UnitOfWork.Api;

namespace Support.DataModelRepository.IndexManipulator
{
    internal class CategoryIndexManipulator<TAggregateDatabaseModel,
            TLookupDatabaseModel>
        : ICategoryIndexManipulator<TAggregateDatabaseModel,
            TLookupDatabaseModel>
        where TAggregateDatabaseModel : class, IAggregateDataModel
        where TLookupDatabaseModel : IRepositoryLookup
    {
        public CategoryIndexManipulator(
            IAggregateToLookupMapper<TAggregateDatabaseModel,
                TLookupDatabaseModel> mapper)
        {
            _mapper = mapper;
        }

        /// <inheritdoc />
        public void Upsert(
            CategoryIndex<TLookupDatabaseModel> nonDeletedCategoryIndex,
            TAggregateDatabaseModel aggregate)
        {
            var lookup = _mapper.ToLookup(aggregate);

            lookup.Key = aggregate.Key;

            nonDeletedCategoryIndex.Lookups =
                nonDeletedCategoryIndex.Lookups
                    .Where(l => l.Key != aggregate.Key)
                    .Append(lookup)
                    .ToList();
        }

        /// <inheritdoc />
        public void Delete(
            CategoryIndex<TLookupDatabaseModel> nonDeletedCategoryIndex,
            CategoryIndex<TLookupDatabaseModel> deletedCategoryIndex,
            string key,
            DateTime timeStamp)
        {
            AssertContainsKey(nonDeletedCategoryIndex, key, false);

            var item = GetItem(nonDeletedCategoryIndex, key);

            item.IsDeleted = true;
            item.DeletedTimeStamp = timeStamp.ToString("o");

            RemoveItemIfFound(nonDeletedCategoryIndex, key);

            AppendItem(deletedCategoryIndex, item);
        }

        /// <inheritdoc />
        public void Restore(
            CategoryIndex<TLookupDatabaseModel> nonDeletedCategoryIndex,
            CategoryIndex<TLookupDatabaseModel> deletedCategoryIndex,
            string key)
        {
            AssertContainsKey(deletedCategoryIndex, key, true);

            var item = GetItem(deletedCategoryIndex, key);

            item.IsDeleted = false;
            item.DeletedTimeStamp = DateTime.MinValue.ToString("o");

            RemoveItemIfFound(deletedCategoryIndex, key);

            AppendItem(nonDeletedCategoryIndex, item);
        }

        private TLookupDatabaseModel GetItem(
            CategoryIndex<TLookupDatabaseModel> index, string key)
        {
            return index.Lookups.First(l => l.Key == key);
        }

        private void RemoveItemIfFound(
            CategoryIndex<TLookupDatabaseModel> index,
            string key)
        {
            index.Lookups = index.Lookups.Where(l => l.Key != key).ToList();
        }

        private void AppendItem(CategoryIndex<TLookupDatabaseModel> index,
            TLookupDatabaseModel item)
        {
            index.Lookups = index.Lookups.Append(item);
        }

        private void AssertContainsKey(
            CategoryIndex<TLookupDatabaseModel> index, string key,
            bool isDeletedIndex)
        {
            if (index.Lookups.All(l => l.Key != key))
            {
                throw new LookupNotFoundInCategoryIndexException(key,
                    isDeletedIndex);
            }
        }

        private readonly
            IAggregateToLookupMapper<TAggregateDatabaseModel,
                TLookupDatabaseModel> _mapper;
    }
}