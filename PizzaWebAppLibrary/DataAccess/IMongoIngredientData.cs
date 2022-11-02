
namespace PizzaWebAppLibrary.DataAccess;

public interface IMongoIngredientData
{
    Task CreateIngredient(IngredientModel ingredient);
    Task<List<IngredientModel>> GetAllIngredients();
}