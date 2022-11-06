namespace PizzaWebAppLibrary.DataAccess;

public class MongoOrderStatusesData
{
    private IMongoCollection<OrderStatusModel> orderStatuses;
    private IMemoryCache cache;
    private const string CacheName = "OrderStatusesData";

    public MongoOrderStatusesData(IDbConnection _db, IMemoryCache _cache)
    {
        cache = _cache;
        orderStatuses = _db.OrderStatusCollection;
    }

    public async Task<List<OrderStatusModel>> GetAllOrderStatuses()
    {
        var output = cache.Get<List<OrderStatusModel>>(CacheName);

        if (output == null)
        {
            var temp = await orderStatuses.FindAsync(_ => true);
            output = temp.ToList();

            cache.Set(CacheName, output, TimeSpan.FromDays(1));
        }

        return output;
    }

    public Task CreateOrderStatus(OrderStatusModel orderStatus)
    {
        return orderStatuses.InsertOneAsync(orderStatus);
    }

    public Task DeleteOrderStatus(string id)
    {
        return orderStatuses.DeleteOneAsync(x => x.Id == id);
        cache.Remove(CacheName);
    }
}
