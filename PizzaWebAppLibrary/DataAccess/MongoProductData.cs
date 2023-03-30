using System.Reflection.Metadata;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

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

    public async Task CreateProduct<TModel>(TModel product) where TModel : ProductModel
    {
        await products.InsertOneAsync(product);
        cache.Remove(CacheName);
    }

    public async Task UpdateProduct<TModel>(TModel product) where TModel : ProductModel
    {
        await products.ReplaceOneAsync(x => x.Id == product.Id, product);
        cache.Remove(CacheName);
    }

    public async Task DeleteProduct<TModel>(TModel product) where TModel : ProductModel
    {
        await products.DeleteOneAsync(x => x.Id == product.Id);
        cache.Remove(CacheName);
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


    //public async Task<BsonDocument> GetProduct(string id)
    //{
    //    var collectionName = dbConn.ProductCollectionName;
    //    var db = dbConn.db;
    //    var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));

    //    var collection = db.GetCollection<BsonDocument>(collectionName);
    //    var cursor = await collection.FindAsync(filter);
    //    var document = cursor.FirstOrDefault();

    //    return document;
    //    var documentType = document["_t"].ToString();
    //}




    public async Task<TModel> GetProduct<TModel>(TModel memberOfDesiredType) where TModel : ProductModel
    {
        var temp = await GetAllProducts();
        return (TModel)temp.Where(x => x.Id == memberOfDesiredType.Id).FirstOrDefault();

        //var collectionName = dbConn.ProductCollectionName;
        //var db = dbConn.db;
        //var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));

        //var collection = db.GetCollection<BsonDocument>(collectionName);
        //var cursor = await collection.FindAsync(filter);
        //var document = cursor.FirstOrDefault();
        //var documentType = document["_t"].ToString();


        //switch (documentType)
        //{
        //    case "ProductDrinkModel":
        //        object result0 = BsonSerializer.Deserialize<ProductDrinkModel>(document);
        //        return (TModel)result0;
        //    case "ProductPizzaModel":
        //        object result1 = BsonSerializer.Deserialize<ProductPizzaModel>(document);
        //        return (TModel)result1;
        //    default:
        //        object result2 = BsonSerializer.Deserialize<ProductModel>(document);
        //        return (TModel)result2;
        //}
        //var product = products.FindAsync(x => x.Id == id).ToBsonDocument();
        //var productType = product["_t"].AsString;


        //var collectionName = dbConn.ProductCollectionName;
        //var db = dbConn.db;
        //var filter = new BsonDocument("_id", id);

        //dynamic collection = productType switch
        //{
        //    "ProductDrinkModel" => db.GetCollection<ProductDrinkModel>(collectionName).FindAsync(x => x.Id == id).ToBsonDocument(),
        //    "ProductPizzaModel" => db.GetCollection<ProductPizzaModel>(collectionName).FindAsync(x => x.Id == id).ToBsonDocument(),
        //    _ => db.GetCollection<ProductModel>(collectionName).FindAsync(x => x.Id == id).ToBsonDocument(),
        //};

        //var a = collection;

        //switch (productType)
        //{
        //    case "ProductDrinkModel":
        //        var a = db.GetCollection<ProductDrinkModel>(collectionName);
        //        var outputA = await a.FindAsync(filter);
        //        return (TModel)outputA.FirstOrDefault();
        //    break;
        //    case "ProductPizzaModel":              
        //        var b = db.GetCollection<ProductPizzaModel>(collectionName);
        //        break;
        //    default:
        //        var c = db.GetCollection<ProductModel>(collectionName);
        //        break;
        //}



        
    }



}