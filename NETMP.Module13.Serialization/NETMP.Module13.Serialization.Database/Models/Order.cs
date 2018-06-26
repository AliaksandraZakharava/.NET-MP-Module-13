using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Task.DB
{
    using System;
    using System.CodeDom;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Reflection;

    [DataContract]
    public class Order
    {
        public Order()
        {
            Order_Details = new HashSet<Order_Detail>();
        }


        [DataMember(Name = "id")]
        public int OrderID { get; set; }

        [DataMember(Name = "customer_id")]
        [StringLength(5)]
        public string CustomerID { get; set; }

        [DataMember(Name = "employee_id")]
        public int? EmployeeID { get; set; }

        [DataMember(Name = "order_date")]
        public DateTime? OrderDate { get; set; }

        [DataMember(Name = "required_date")]
        public DateTime? RequiredDate { get; set; }

        [DataMember(Name = "shipped_date")]
        public DateTime? ShippedDate { get; set; }

        [DataMember(Name = "ship_via")]
        public int? ShipVia { get; set; }

        [DataMember(Name = "freight")]
        [Column(TypeName = "money")]
        public decimal? Freight { get; set; }

        [DataMember(Name = "ship_name")]
        [StringLength(40)]
        public string ShipName { get; set; }

        [DataMember(Name = "ship_address")]
        [StringLength(60)]
        public string ShipAddress { get; set; }

        [DataMember(Name = "ship_city")]
        [StringLength(15)]
        public string ShipCity { get; set; }

        [DataMember(Name = "ship_region")]
        [StringLength(15)]
        public string ShipRegion { get; set; }

        [DataMember(Name = "ship_postal_code")]
        [StringLength(10)]
        public string ShipPostalCode { get; set; }

        [DataMember(Name = "ship_country")]
        [StringLength(15)]
        public string ShipCountry { get; set; }

        [DataMember(Name = "customer")]
        public virtual Customer Customer { get; set; }

        [DataMember(Name = "employee")]
        public virtual Employee Employee { get; set; }

        [DataMember(Name = "order_details")]
        public virtual HashSet<Order_Detail> Order_Details { get; set; }

        [DataMember(Name = "shipper")]
        public virtual Shipper Shipper { get; set; }
    }

    [DataContract(Name = "order")]
    public class OrderSurrogate
    {
        [DataMember(Name="xml_data")]
        public string XmlData { get; set; }
    }

    public class OrderDataContractSurrogate : IDataContractSurrogate
    {
        public Type GetDataContractType(Type type)
        {
            Console.WriteLine("OrderDataContractSurrogate.GetDataContractType() invoked.");

            if (typeof(Order).IsAssignableFrom(type))
            {
                return typeof(OrderSurrogate);
            }
            return type;
        }

        public object GetObjectToSerialize(object obj, Type targetType)
        {
            Console.WriteLine("OrderDataContractSurrogate.GetObjectToSerialize() invoked.");

            if (obj is Order order)
            {
                var serializer = new XmlSerializer(typeof(Order));
                var writer = new StringWriter();
                var orderSurrogate = new OrderSurrogate();

                serializer.Serialize(writer, order);
                orderSurrogate.XmlData = writer.ToString();

                return orderSurrogate;
            }

            return obj;
        }

        public object GetDeserializedObject(object obj, Type targetType)
        {
            Console.WriteLine("OrderDataContractSurrogate.GetDeserializedObject() invoked.");

            if (obj is OrderSurrogate)
            {
                var orderSurrogate  = (OrderSurrogate)obj;
                var serializer = new XmlSerializer(typeof(Order));

                return (Order)serializer.Deserialize(new StringReader(orderSurrogate.XmlData));
            }

            return obj;
        }

        public Type GetReferencedTypeOnImport(string typeName, string typeNamespace, object customData)
        {
            Console.WriteLine("OrderDataContractSurrogate.GetReferencedTypeOnImport() invoked.");

            if (typeName.Equals("OrderSurrogate"))
            {
                return typeof(Order);
            }

            return null;
        }

        public CodeTypeDeclaration ProcessImportedType(CodeTypeDeclaration typeDeclaration, CodeCompileUnit compileUnit)
        {
            Console.WriteLine("OrderDataContractSurrogate.ProcessImportedType() invoked.");

            return typeDeclaration;
        }

        public object GetCustomDataToExport(MemberInfo memberInfo, Type dataContractType)
        {
            return null;
        }

        public object GetCustomDataToExport(Type clrType, Type dataContractType)
        {
            return null;
        }

        public void GetKnownCustomDataTypes(Collection<Type> customDataTypes)
        {
            return;
        }
    }

    public class OrdersCollection
    {
        public IEnumerable<Order> Orders { get; set; }
    }

    public class OrderDataContractSurrogateHelper<TData>
    {
        private readonly DataContractSerializer _serializer;

        public OrderDataContractSurrogateHelper()
        {
            var knownTypes = new List<Type> {typeof(Order), typeof(OrderSurrogate), typeof(OrdersCollection),
                typeof(Customer), typeof(Employee), typeof(Order_Detail), typeof(Shipper)};

            _serializer = new DataContractSerializer(typeof(Order), knownTypes, Int16.MaxValue, false, true, new OrderDataContractSurrogate());
        }

        public TData SerializeAndDeserialize(TData data, bool showResult = true)
        {
            var stream = new MemoryStream();

            Console.WriteLine("Start serialization");

            _serializer.WriteObject(stream, data);

            Console.WriteLine("Serialization finished");

            if (showResult)
            {
                var r = Console.OutputEncoding.GetString(stream.GetBuffer(), 0, (int)stream.Length);
                Console.WriteLine(r);
            }

            stream.Seek(0, SeekOrigin.Begin);

            Console.WriteLine("Start deserialization");

            var result = (TData) _serializer.ReadObject(stream);

            Console.WriteLine("Deserialization finished");

            return result;
        }
    }
}
