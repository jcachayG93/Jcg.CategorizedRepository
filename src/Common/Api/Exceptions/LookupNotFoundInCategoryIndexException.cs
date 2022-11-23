using Support.UnitOfWork.Api.Exceptions;

namespace Common.Api.Exceptions
{
    public class LookupNotFoundInCategoryIndexException : RepositoryException
    {
        public LookupNotFoundInCategoryIndexException(
            string key, bool isDeleted) : base(
            CreateMessage(key, isDeleted))
        {
        }

        private static string CreateMessage(string key, bool isDeleted)
        {
            var indexName = isDeleted
                ? "Deleted items category index"
                : "Non deleted items category index";

            return $"Lookup with key: {key} was not found in the {indexName}";
        }
    }
}