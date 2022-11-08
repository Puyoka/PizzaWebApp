namespace PizzaWebAppLibrary.DataAccess;

public class MongoOrderData : IOrderData
{
    private IMongoCollection<OrderModel> orders;
    private IMemoryCache cache;
    private const string CacheName = "OrdersData";

    public MongoOrderData(IDbConnection _db, IMemoryCache _cache)
    {
        cache = _cache;
        orders = _db.OrderCollection;
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
}
