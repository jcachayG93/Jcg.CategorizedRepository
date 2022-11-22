namespace Testing.Common.Extensions
{
    public static class ToCollectionExtension
    {
        public static IEnumerable<T> ToCollection<T>(
            this T item, params T[] additionalItems)
        {
            return additionalItems.Append(item).ToList();
        }
    }
}