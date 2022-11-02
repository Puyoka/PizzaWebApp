namespace PizzaWebAppLibrary.Models;


[BsonDiscriminator(Required = true, RootClass = true)]
[BsonKnownTypes(typeof(Pizzas), typeof(Drinks))]
public class CategoryModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Name { get; set; }
}

public class Pizzas : CategoryModel
{
    public List<IngredientModel> Ingrediens { get; set; }
}

public class Drinks : CategoryModel
{
    public string Size { get; set; }
}