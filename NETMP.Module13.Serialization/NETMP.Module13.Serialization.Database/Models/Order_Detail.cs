using System;
using System.Runtime.Serialization;

namespace Task.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Serializable]
    [Table("Order Details")]
    public class Order_Detail
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? OrderID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? ProductID { get; set; }

        [Column(TypeName = "money")]
        public decimal UnitPrice { get; set; }

        public short Quantity { get; set; }

        public float Discount { get; set; }

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }
    }

    public class OrderDetailSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            var details = (Order_Detail)obj;

            info.AddValue("id", details.OrderID);
            info.AddValue("unit_price", details.UnitPrice);
            info.AddValue("quantity", details.Quantity);
            info.AddValue("discount", details.Discount);
            info.AddValue("order", details.Order);
            info.AddValue("product", details.Product);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            var details = (Order_Detail)obj;

            details.OrderID = info.GetInt32("id");
            details.UnitPrice = info.GetDecimal("unit_price");
            details.Quantity = (short)info.GetValue("quantity", typeof(short));
            details.Discount = (float)info.GetValue("quantity", typeof(float));
            details.Order = (Order)info.GetValue("order", typeof(Order));
            details.Product = (Product)info.GetValue("product", typeof(Product));

            details.OrderID = details.Order?.OrderID;
            details.ProductID = details.Product?.ProductID;

            return details;
        }
    }
}
