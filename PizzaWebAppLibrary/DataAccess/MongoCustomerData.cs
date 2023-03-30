using static MongoDB.Driver.WriteConcern;
using MongoDB.Driver.Linq;

namespace PizzaWebAppLibrary.DataAccess;

public class MongoCustomerData : ICustomerData
{
    private IMongoCollection<CustomerModel> customers;
    private IDbConnection dbConn;
    private const string CacheName = "CustomersData";

    public MongoCustomerData(IDbConnection db)
    {
        customers = db.CustomerCollection;
        dbConn = db;
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

    public async Task<CustomerModel> GetCustomerFromAuthentication(string objectId)
    {
        var result = await customers.FindAsync(u => u.ObjectIdentifier == objectId);
        return result.FirstOrDefault();
    }

    public Task UpdateCustomer(CustomerModel customer)
    {
        return customers.ReplaceOneAsync(x => x.Id == customer.Id, customer); //update?
    }

    public async Task<List<IngredientModel>> GetPizzaIngredientsFromShoppingcart(CustomerModel customer, int shoppingcartIndex)
    {
        var collectionName = dbConn.CustomerCollectionName;
        var db = dbConn.db;
        var collection = db.GetCollection<BsonDocument>(collectionName);

        var filter = Builders<BsonDocument>.Filter.Eq("ObjectIdentifier", customer.ObjectIdentifier);
        var projection = Builders<BsonDocument>.Projection.Include("ShoppingCart.Ingrediens");
        var documents = collection.Find(filter).Project(projection).ToList();

        var ingredients = new List<IngredientModel>();

        var bsonArray = documents[0]["ShoppingCart"].AsBsonArray;
        if (bsonArray.Count -1 >= shoppingcartIndex)
        {
            var bsonDocument = bsonArray[shoppingcartIndex].AsBsonDocument;
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
