namespace PizzaWebAppLibrary.DataAccess;

public interface IProductData
{
    Task CreateProduct<TModel>(TModel product) where TModel : ProductModel;
    Task DeleteProduct<TModel>(TModel product) where TModel : ProductModel;
    Task<List<ProductModel>> GetAllProducts();
    Task<List<TModel>> GetAvailabelProductsByType<TModel>(TModel memberOfDesiredType, bool available = true);
    Task<List<ProductModel>> GetAvailableProducts(bool availabel = true);
    Task<TModel> GetProduct<TModel>(TModel memberOfDesiredType) where TModel : ProductModel;
    Task<List<TModel>> GetProductsByType<TModel>(TModel memberOfDesiredType);
    Task UpdateProduct<TModel>(TModel product) where TModel : ProductModel;
}