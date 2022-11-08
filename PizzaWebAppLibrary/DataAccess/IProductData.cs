namespace PizzaWebAppLibrary.DataAccess;

public interface IProductData
{
    Task CreateProduct<TModel>(TModel product) where TModel : ProductModel;
    Task<List<ProductModel>> GetAllAvailableProducts(bool availabel = true);
    Task<List<ProductModel>> GetAllProducts();
    Task<List<TModel>> GetAvailabelProductsByType<TModel>(TModel memberOfDesiredType, bool available = true);
    Task<List<TModel>> GetProductsByType<TModel>(TModel memberOfDesiredType);
}