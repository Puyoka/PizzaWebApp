using Microsoft.Extensions.Configuration;


namespace PizzaWebAppLibrary.DataAccess;
public class DbConnection : IDbConnection
{
    private readonly IConfiguration _config;

    public string ConnectionString { get; private set; }
    public string DatabaseName { get; private set; }
    public string CustomerCollectionName { get; private set; }
    public string OrderCollectionName { get; private set; }
    public string OrderStatusCollectionName { get; private set; }
    public string ProductCollectionName { get; private set; }
    public string CategoryCollectionName { get; private set; }
    public string IngredientCollectionName { get; private set; }

    public MongoClient Client { get; private set; }
    public IMongoDatabase db { get; private set; }
    public IMongoCollection<CustomerModel> CustomerCollection { get; private set; }
    public IMongoCollection<OrderModel> OrderCollection { get; private set; }
    public IMongoCollection<OrderStatusModel> OrderStatusCollection { get; private set; }
    public IMongoCollection<ProductModel> ProductCollection { get; private set; }
    public IMongoCollection<CategoryModel> CategoryCollection { get; private set; }
    public IMongoCollection<IngredientModel> IngredientCollection { get; private set; }

    public DbConnection(IConfiguration config)
    {
        _config = config.GetSection("PizzaWebAppDatabase");

        ConnectionString = _config[nameof(ConnectionString)]; 
        DatabaseName = _config[nameof(DatabaseName)]!;
        CustomerCollectionName = _config[nameof(CustomerCollectionName)]!;
        OrderCollectionName = _config[nameof(OrderCollectionName)]!;
        OrderStatusCollectionName = _config[nameof(OrderStatusCollectionName)]!;
        ProductCollectionName = _config[nameof(ProductCollectionName)]!;
        CategoryCollectionName = _config[nameof(CategoryCollectionName)]!;
        IngredientCollectionName = _config[nameof(IngredientCollectionName)]!;

        Client = new MongoClient(ConnectionString);
        db = Client.GetDatabase(DatabaseName);

        CustomerCollection = db.GetCollection<CustomerModel>(CustomerCollectionName);
        OrderCollection = db.GetCollection<OrderModel>(OrderCollectionName);
        OrderStatusCollection = db.GetCollection<OrderStatusModel>(OrderStatusCollectionName);
        ProductCollection = db.GetCollection<ProductModel>(ProductCollectionName);
        CategoryCollection = db.GetCollection<CategoryModel>(CategoryCollectionName);
        IngredientCollection = db.GetCollection<IngredientModel>(IngredientCollectionName);
    }



    //public async Task CreateUser(CustomerModel customer)
    //{
    //    await Customers.InsertOneAsync(customer);
    //}

    //public async Task CreateProduct(ProductModel product)
    //{
    //    await Products.InsertOneAsync(product);
    //}

    //public async Task Create<TModel, TCollection>(TModel item, TCollection collection) where TCollection : IMongoCollection<TModel>
    //{
    //    await collection.InsertOneAsync(item);
    //}

}
