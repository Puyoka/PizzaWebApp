namespace PizzaWebAppLibrary.Models;
public class IngredientModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public List<string> Allergens { get; set; }
}
