namespace PizzaWebAppLibrary.DataAccess;
public class MongoIngredientData
{
    private IMongoCollection<IngredientModel> ingredients;
    private IMemoryCache cache;
    private const string CacheName = "IngredientsData";

    public MongoIngredientData(IDbConnection db, IMemoryCache _cache)
    {
        cache = _cache;
        ingredients = db.IngredientCollection;
    }

    public async Task<List<IngredientModel>> GetAllIngredients()
    {
        var result = cache.Get<List<IngredientModel>>(CacheName);

        if (result == null)
        {
            var temp = await ingredients.FindAsync(_ => true);
            result = temp.ToList();

            cache.Set(CacheName, result, TimeSpan.FromDays(1));
        }

        return result;
    }

    public async Task<IngredientModel> GetIngredient(string id)
    {
        var result = await GetAllIngredients();
        return result.Where(x => x.Id == id).FirstOrDefault();
    }

    public async Task CreateIngredient(IngredientModel ingredient)
    {
        await ingredients.InsertOneAsync(ingredient);
        cache.Remove(CacheName);
    }

    public async Task DeleteIngredient(string id)
    {
        await ingredients.DeleteOneAsync(x => x.Id == id);
        cache.Remove(CacheName);
    }

    public async Task UpdateIngredient(IngredientModel ingredient)
    {
        await ingredients.ReplaceOneAsync(x => x.Id == ingredient.Id, ingredient);
        cache.Remove(CacheName);
    }

}