﻿namespace PizzaWebAppLibrary.DataAccess;

public interface ICustomerData
{
    Task CreateCustomer(CustomerModel customer);
    Task<List<CustomerModel>> GetAllCustomers();
    Task<CustomerModel> GetCustomer(string id);
    Task<CustomerModel> GetCustomerFromAuthentication(string objectId);
    Task UpdateCustomer(CustomerModel customer);
}