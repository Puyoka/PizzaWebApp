namespace PizzaWebAppLibrary.DataAccess;
public class MongoCategoryData : ICategoryData
{
    private IMongoCollection<CategoryModel> categories;
    private IMemoryCache cache;
    private const string CacheName = "CategoriesData";

    public MongoCategoryData(IDbConnection db, IMemoryCache _cache)
    {
        cache = _cache;
        categories = db.CategoryCollection;
    }

    public async Task<List<CategoryModel>> GetAllCategories()
    {
        var result = cache.Get<List<CategoryModel>>(CacheName);

        if (result == null)
        {
            var temp = await categories.FindAsync(_ => true);
            result = temp.ToList();

            cache.Set(CacheName, result, TimeSpan.FromDays(1));
        }

        return result;
    }

    public Task CreateCategory(CategoryModel category)
    {
        cache.Remove(CacheName);
        return categories.InsertOneAsync(category);
    }

}
