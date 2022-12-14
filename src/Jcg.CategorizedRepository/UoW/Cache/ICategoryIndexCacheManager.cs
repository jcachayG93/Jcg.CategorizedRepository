using Jcg.CategorizedRepository.Api;
using Jcg.CategorizedRepository.Api.Exceptions;

namespace Jcg.CategorizedRepository.UoW.Cache;

internal interface ICategoryIndexCacheManager<TLookup>
{
    /// <summary>
    ///     If the category index was upserted, this will contain the latest version. Null if it was not upserted.
    /// </summary>
    UpsertedItem<CategoryIndex<TLookup>>? UpsertedItem { get; }

    /// <summary>
    ///     Gets the data from the cache, if the data has not been read, it will read it from the database.
    ///     If the database returns null, an exception will be thrown.
    /// </summary>
    /// <returns>The category index</returns>
    /// <exception cref="CategoryIndexIsUninitializedException"></exception>
    Task<CategoryIndex<TLookup>> GetAsync();

    /// <summary>
    ///     True if the index exists in the database
    /// </summary>
    /// <returns></returns>
    Task<bool> IndexExistsAsync();

    /// <summary>
    ///     Replaces the category index in the cache.
    /// </summary>
    /// <param name="categoryIndex">The updated category index</param>
    Task UpsertAsync(CategoryIndex<TLookup> categoryIndex);
}