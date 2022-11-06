using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaWebAppLibrary.Models;
public class OrderModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public BasicCustomerModel Customer { get; set; }
    public List<ProductModel> Products { get; set; }
    public int Total { get; set; }
    public bool PaysWithCash { get; set; }
    public string Description { get; set; }
    public DateTime Date_0 { get; set; }
    public DateTime? Date_1 { get; set; }
    public OrderStatusModel OrderStatus { get; set; }
}
