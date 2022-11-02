using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaWebAppLibrary.DataAccess;
public class MongoIngredientData : IMongoIngredientData
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

    public Task CreateIngredient(IngredientModel ingredient)
    {
        cache.Remove(CacheName);
        return ingredients.InsertOneAsync(ingredient);
    }

}
