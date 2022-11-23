using Support.UnitOfWork.Api;

namespace IntegrationTests.Common.Types
{
    public class CustomCategoryIndex : CategoryIndex<LookupDataModel>, IClone
    {
        /// <inheritdoc />
        public object Clone()
        {
            return new CustomCategoryIndex()
            {
                Lookups = Lookups.ToList()
            };
        }
    }
}