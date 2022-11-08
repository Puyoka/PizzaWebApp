namespace PizzaWebAppLibrary.DataAccess;

public interface IOrderStatusData
{
    Task CreateOrderStatus(OrderStatusModel orderStatus);
    Task DeleteOrderStatus(string id);
    Task<List<OrderStatusModel>> GetAllOrderStatuses();
}