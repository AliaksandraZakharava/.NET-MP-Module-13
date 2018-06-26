using System;
using System.Runtime.Serialization;

namespace Task.DB
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Serializable]
    public class Product : ISerializable
    {
        public Product()
        {
            Order_Details = new HashSet<Order_Detail>();
        }

        public int ProductID { get; set; }

        [Required]
        [StringLength(40)]
        public string ProductName { get; set; }

        public int? SupplierID { get; set; }

        public int? CategoryID { get; set; }

        [StringLength(20)]
        public string QuantityPerUnit { get; set; }

        [Column(TypeName = "money")]
        public decimal? UnitPrice { get; set; }

        public short? UnitsInStock { get; set; }

        public short? UnitsOnOrder { get; set; }

        public short? ReorderLevel { get; set; }

        public bool Discontinued { get; set; }

        public virtual Category Category { get; set; }

        public virtual HashSet<Order_Detail> Order_Details { get; set; }

        public virtual Supplier Supplier { get; set; }

        #region Serialization methods

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("id", ProductID, typeof(int));
            info.AddValue("name", ProductName, typeof(string));
            info.AddValue("quantity_per_unit", QuantityPerUnit, typeof(string));
            info.AddValue("unit_price", UnitPrice, typeof(decimal?));
            info.AddValue("units_in_stock", UnitsInStock, typeof(short?));
            info.AddValue("units_on_order", UnitsOnOrder, typeof(short?));
            info.AddValue("reorder_level", ReorderLevel, typeof(short?));
            info.AddValue("discontinued", Discontinued, typeof(bool));
            info.AddValue("category", Category, typeof(Category));
            info.AddValue("order_details", Order_Details, typeof(HashSet<Order_Detail>));
            info.AddValue("supplier", Supplier, typeof(Supplier));
        }

        public Product(SerializationInfo info, StreamingContext context)
        {
            ProductID = info.GetInt32("id");
            ProductName = info.GetString("name");

            QuantityPerUnit = info.GetString("quantity_per_unit");
            UnitPrice = (decimal?)info.GetValue("unit_price", typeof(decimal?));
            UnitsInStock = (short?)info.GetValue("units_in_stock", typeof(short?));
            UnitsOnOrder = (short?)info.GetValue("units_on_order", typeof(short?));
            ReorderLevel = (short?)info.GetValue("reorder_level", typeof(short?));
            Discontinued = info.GetBoolean("discontinued");

            Category = (Category)info.GetValue("category", typeof(Category));
            Order_Details = (HashSet<Order_Detail>)info.GetValue("order_details", typeof(HashSet<Order_Detail>));
            Supplier = (Supplier)info.GetValue("supplier", typeof(Supplier));

            SupplierID = Supplier?.SupplierID;
            CategoryID = Category?.CategoryID;
        }

        #endregion
    }
}
