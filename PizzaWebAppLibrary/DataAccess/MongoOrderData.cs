namespace PizzaWebAppLibrary.DataAccess;

public class MongoOrderData : IOrderData
{
    private IMongoCollection<OrderModel> orders;
    private IMemoryCache cache;
    private IDbConnection dbConn;
    private const string CacheName = "OrdersData";

    public MongoOrderData(IDbConnection _db, IMemoryCache _cache)
    {
        cache = _cache;
        orders = _db.OrderCollection;
        dbConn = _db;
    }

    public Task CreateOrder(OrderModel order)
    {
        return orders.InsertOneAsync(order);
    }

    public Task UpdateOrderStatus(OrderModel order)
    {
        return orders.ReplaceOneAsync(x => x.Id == order.Id, order);
    }

    public async Task<List<OrderModel>> GetAllOrders()
    {
        var output = cache.Get<List<OrderModel>>(CacheName);

        if (output == null)
        {
            var temp = await orders.FindAsync(_ => true);
            output = temp.ToList();

            cache.Set(CacheName, output, TimeSpan.FromSeconds(30));
        }

        return output;
    }

    public async Task<List<OrderModel>> GetCurrentOrders()
    {
        var allOrders = await GetAllOrders();
        var result = allOrders.Where(x => x.Date_1 == null);

        return result.ToList();
    }

    public async Task<List<OrderModel>> GetCustomerOrders(string customerId)
    {
        var allOrders = await GetAllOrders();
        var result = allOrders.Where(x => x.Customer.Id == customerId);

        return result.ToList();
    }

    public async Task<List<OrderModel>> GetCustomerCurrentOrders(string customerId)
    {
        var allOrders = await GetAllOrders();
        var result = allOrders.Where(x => x.Customer.Id == customerId && x.Date_1 == null);

        return result.ToList();
    }

    public async Task<OrderModel> GetOrder(string orderId)
    {
        var allOrders = await GetAllOrders();
        var result = allOrders.Where(x => x.Id == orderId);

        return result.FirstOrDefault();
    }

    public async Task<List<IngredientModel>> GetPizzaIngredientsFromOrder(OrderModel order, int index)
    {
        var collectionName = dbConn.OrderCollectionName;
        var db = dbConn.db;
        var collection = db.GetCollection<BsonDocument>(collectionName);

        var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(order.Id));
        var projection = Builders<BsonDocument>.Projection.Include("Products.Ingrediens");
        var documents = collection.Find(filter).Project(projection).ToList();

        var ingredients = new List<IngredientModel>();

        var bsonArray = documents[0]["Products"].AsBsonArray;
        if (bsonArray.Count - 1 >= index)
        {
            var bsonDocument = bsonArray[index].AsBsonDocument;
            if (bsonDocument != null)
            {
                var bsonArrayIngredients = bsonDocument["Ingrediens"].AsBsonArray;

                foreach (var bsonValueIngredient in bsonArrayIngredients)
                {
                    var bsonDocumentIngredient = bsonValueIngredient.AsBsonDocument;
                    var ingredient = new IngredientModel
                    {
                        Id = bsonDocumentIngredient["_id"].ToString(),
                        Name = bsonDocumentIngredient["Name"].ToString(),
                        Price = bsonDocumentIngredient["Price"].ToInt32(),
                        Allergens = null//bsonDocumentIngredient["Allergens"].AsBsonArray.Select(x => x.ToString()).ToList()
                    };

                    ingredients.Add(ingredient);

                }
            }

        }
        return ingredients;
    }
}
