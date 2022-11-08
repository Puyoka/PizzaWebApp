namespace PizzaWebAppLibrary.DataAccess;

public class MongoCustomerData : ICustomerData
{
    private IMongoCollection<CustomerModel> customers;

    public MongoCustomerData(IDbConnection db)
    {
        customers = db.CustomerCollection;
    }

    public Task CreateCustomer(CustomerModel customer)
    {
        return customers.InsertOneAsync(customer);
    }

    public async Task<List<CustomerModel>> GetAllCustomers()
    {
        var output = await customers.FindAsync(_ => true);
        return output.ToList();
    }

    public async Task<CustomerModel> GetCustomer(string id)
    {
        var output = await customers.FindAsync(x => x.Id == id);
        return output.FirstOrDefault();
    }

    public Task UpdateCustomer(CustomerModel customer)
    {
        return customers.ReplaceOneAsync(x => x.Id == customer.Id, customer);
    }
}
