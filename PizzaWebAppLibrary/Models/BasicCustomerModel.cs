namespace PizzaWebAppLibrary.Models;

public class BasicCustomerModel
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public AddressModel Address { get; set; }
    public int PhoneNumber { get; set; }

}