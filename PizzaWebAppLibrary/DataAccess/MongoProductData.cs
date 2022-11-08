namespace PizzaWebAppLibrary.DataAccess;
public class MongoProductData : IProductData
{
    private IMongoCollection<ProductModel> products;
    private IMemoryCache cache;
    private IDbConnection dbConn;
    private const string CacheName = "ProductsData";

    public MongoProductData(IDbConnection _db, IMemoryCache _cache)
    {
        cache = _cache;
        products = _db.ProductCollection;
        dbConn = _db;
    }

    public Task CreateProduct<TModel>(TModel product) where TModel : ProductModel
    {
        return products.InsertOneAsync(product);
    }

    public async Task<List<ProductModel>> GetAllProducts()
    {
        var result = cache.Get<List<ProductModel>>(CacheName);

        if (result == null)
        {
            var temp = await products.FindAsync(_ => true);
            result = temp.ToList();

            cache.Set(CacheName, result, TimeSpan.FromHours(1));
        }

        return result;
    }

    public async Task<List<ProductModel>> GetAvailableProducts(bool availabel = true)
    {
        var temp = await GetAllProducts();
        return temp.Where(x => x.Available == availabel).ToList();
    }

    public async Task<List<TModel>> GetProductsByType<TModel>(TModel memberOfDesiredType)
    {
        var collectionName = dbConn.ProductCollectionName;
        var db = dbConn.db;
        var filter = new BsonDocument("_t", memberOfDesiredType.GetType().Name);
        var collection = db.GetCollection<TModel>(collectionName);

        var result = await collection.FindAsync(filter);
        return result.ToList();
    }

    public async Task<List<TModel>> GetAvailabelProductsByType<TModel>(TModel memberOfDesiredType, bool available = true)
    {
        var collectionName = dbConn.ProductCollectionName;
        var db = dbConn.db;
        var builder = Builders<TModel>.Filter;
        var filter = builder.Eq("_t", memberOfDesiredType.GetType().Name) & builder.Eq("Available", available);
        var collection = db.GetCollection<TModel>(collectionName);

        var result = await collection.FindAsync(filter);
        return result.ToList();
    }

    //public async Task<TModel> GetProduct<TModel>(string id)
    //{
    //    var product = products.FindAsync(x => x.Id == id).ToBsonDocument();
    //    var productType = product["_t"].AsString;


    //    var collectionName = dbConn.ProductCollectionName;
    //    var db = dbConn.db;
    //    var filter = new BsonDocument("_id", id);
    //    var collection = db.GetCollection < Type.GetType(productType) > (collectionName);

    //    var result = await collection.FindAsync(filter);
    //    var a = result.FirstOrDefault();


    //    return null;
    //}
}