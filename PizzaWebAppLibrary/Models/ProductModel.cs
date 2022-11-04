namespace PizzaWebAppLibrary.Models;

[BsonDiscriminator(Required = true, RootClass = true)]
[BsonKnownTypes(typeof(ProductPizzaModel), typeof(ProductDrinkModel))]
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


public class ProductPizzaModel : ProductModel
{
    public List<IngredientModel> Ingrediens { get; set; }
}

public class ProductDrinkModel : ProductModel
{
    public string Size { get; set; }
}