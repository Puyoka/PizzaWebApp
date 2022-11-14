namespace PizzaWebAppLibrary.DataAccess;

public interface IProductData
{
    Task CreateProduct<TModel>(TModel product) where TModel : ProductModel;
    Task<List<ProductModel>> GetAllProducts();
    Task<List<TModel>> GetAvailabelProductsByType<TModel>(TModel memberOfDesiredType, bool available = true);
    Task<List<ProductModel>> GetAvailableProducts(bool availabel = true);
    Task<BsonDocument> GetProduct(string id);
    Task<TModel> GetProduct<TModel>(string id) where TModel : class;
    Task<List<TModel>> GetProductsByType<TModel>(TModel memberOfDesiredType);
}