namespace PizzaWebAppLibrary.Models;

public class CustomerModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Email { get; set; }
    public AddressModel Address { get; set; }
    public int PhoneNumber { get; set; }
    public List<ProductModel> ShoppingCart { get; set; }

}
