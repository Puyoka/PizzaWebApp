﻿namespace PizzaWebAppLibrary.DataAccess;

public interface IOrderData
{
    Task CreateOrder(OrderModel order);
    Task<List<OrderModel>> GetAllOrders();
    Task<List<OrderModel>> GetCurrentOrders();
    Task<List<OrderModel>> GetCustomerCurrentOrders(string customerId);
    Task<List<OrderModel>> GetCustomerOrders(string customerId);
    Task<OrderModel> GetOrder(string orderId);
    Task<List<IngredientModel>> GetPizzaIngredientsFromOrder(OrderModel order, int index);
    Task UpdateOrderStatus(OrderModel order);
}