
namespace PizzaWebAppLibrary.DataAccess;

public interface IDbConnection
{
    IMongoCollection<CategoryModel> CategoryCollection { get; }
    string CategoryCollectionName { get; }
    MongoClient Client { get; }
    string ConnectionString { get; }
    string CustomerCollectionName { get; }
    IMongoCollection<CustomerModel> CustomerCollection { get; }
    string DatabaseName { get; }
    IMongoDatabase db { get; }
    string IngredientCollectionName { get; }
    IMongoCollection<IngredientModel> IngredientCollection { get; }
    string OrderCollectionName { get; }
    IMongoCollection<OrderModel> OrderCollection { get; }
    string OrderStatusCollectionName { get; }
    IMongoCollection<OrderStatusModel> OrderStatusCollection { get; }
    string ProductCollectionName { get; }
    IMongoCollection<ProductModel> ProductCollection { get; }
}