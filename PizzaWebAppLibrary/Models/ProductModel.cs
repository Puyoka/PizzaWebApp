namespace PizzaWebAppLibrary.Models;

public class ProductModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public CategoryModel Category { get; set; }
    public bool Available { get; set; }
}