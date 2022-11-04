namespace PizzaWebAppLibrary.DataAccess;

public interface IIngredientData
{
    Task CreateIngredient(IngredientModel ingredient);
    Task DeleteIngredient(IngredientModel ingredient);
    Task<List<IngredientModel>> GetAllIngredients();
    Task<IngredientModel> GetIngredient(string id);
    Task UpdateIngredient(IngredientModel ingredient);
}