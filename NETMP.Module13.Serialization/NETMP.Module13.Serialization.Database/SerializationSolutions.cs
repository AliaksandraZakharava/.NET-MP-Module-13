using Microsoft.VisualStudio.TestTools.UnitTesting;
using Task.DB;
using Task.TestHelpers;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;

namespace Task
{
    [TestClass]
    public class SerializationSolutions
    {
        Northwind dbContext;

        [TestInitialize]
        public void Initialize()
        {
            dbContext = new Northwind();
        }

        [TestMethod]
        public void SerializationCallbacks()
        {
            dbContext.Configuration.ProxyCreationEnabled = false;

            var tester = new XmlDataContractSerializerTester<IEnumerable<Category>>(new NetDataContractSerializer(), true);

            var objectContext = (dbContext as IObjectContextAdapter).ObjectContext;

            var categories = dbContext.Categories.Take(3).ToList();

            categories.ForEach(category => objectContext.LoadProperty(category, c => c.Products));

            tester.SerializeAndDeserialize(categories);
        }

        [TestMethod]
        public void ISerializable()
        {
            dbContext.Configuration.ProxyCreationEnabled = false;

            var tester = new XmlDataContractSerializerTester<IEnumerable<Product>>(new NetDataContractSerializer(), true);

            var objectContext = (dbContext as IObjectContextAdapter).ObjectContext;

            var products = dbContext.Products.Take(3).ToList();

            products.ForEach(product => objectContext.LoadProperty(product, c => c.Order_Details));
            products.ForEach(product => objectContext.LoadProperty(product, c => c.Supplier));

            tester.SerializeAndDeserialize(products);
        }


        [TestMethod]
        public void ISerializationSurrogate()
        {
            dbContext.Configuration.ProxyCreationEnabled = false;

            var tester = new XmlDataContractSerializerTester<IEnumerable<Order_Detail>>(new NetDataContractSerializer(), true);

            var objectContext = (dbContext as IObjectContextAdapter).ObjectContext;

            var orderDetails = dbContext.Order_Details.Take(3).ToList();

            orderDetails.ForEach(product => objectContext.LoadProperty(product, c => c.Order));
            orderDetails.ForEach(product => objectContext.LoadProperty(product, c => c.Product));

            tester.SerializeAndDeserialize(orderDetails);
        }

        [TestMethod]
        public void IDataContractSurrogate()
        {
            dbContext.Configuration.ProxyCreationEnabled = false;

            var serializer = new OrderDataContractSurrogateHelper<OrdersCollection>();

            var objectContext = (dbContext as IObjectContextAdapter).ObjectContext;

            var orders = dbContext.Orders.Take(3).ToList();

            orders.ForEach(order => objectContext.LoadProperty(order, c => c.Employee));
            //orders.ForEach(order => objectContext.LoadProperty(order, c => c.Customer));
            //orders.ForEach(order => objectContext.LoadProperty(order, c => c.Order_Details));
            //orders.ForEach(order => objectContext.LoadProperty(order, c => c.Shipper));

            serializer.SerializeAndDeserialize(new OrdersCollection {Orders = orders});
        }
    }
}
