using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NETMP.Module13.Serialization.Basic;

namespace NETMP.Module13.Serialization.Tests
{
    [TestClass]
    public class SerializationTests
    {
        [TestMethod]
        public void BasicSerialization_DeserializationTest()
        {
            var serializator = new XmlSerializer(typeof(Catalog));
            Catalog catalog;

            using (var fs = new FileStream("../../books.xml", FileMode.Open))
            {
                catalog = (Catalog)serializator.Deserialize(fs);
            }

            Assert.IsNotNull(catalog);
            Assert.IsTrue(catalog.Books.Any());
        }

        [TestMethod]
        public void BasicSerialization_SerializationTest()
        {
            var fileName = "BasicSerialization_SerializationTest.xml";
            Catalog deserializedCatalog;
            var catalog = new Catalog
            {
                Date = DateTime.Now,
                Books = new List<Book>
                {
                    new Book
                    {
                        Author = "Joseph Albahari, Ben Albahari",
                        Description = "When you have questions about C# 7.0 or the .NET CLR and its core Framework assemblies, this bestselling guide has the answers you need. Since its debut in 2000, C# has become a language of unusual flexibility and breadth, but its continual growth means there’s always more to learn.",
                        Genre = Genre.Computer,
                        Id = "149dgfdsg1987oabbcd50",
                        ISBN = "978-1491987650",
                        PublishDate = DateTime.Now,
                        Publisher = "Amazone books",
                        RegistrationDate = DateTime.Now,
                        Title = "C# 7.0 in a Nutshell: The Definitive Reference"
                    }
                }
            };

            var serializator = new XmlSerializer(typeof(Catalog));

            // to omit adding standard xmlns:xsi and xmlns:xsd
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, "http://library.by/catalog");

            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                serializator.Serialize(fs, catalog, namespaces);
            }

            using (var fs = new FileStream(fileName, FileMode.Open))
            {
                deserializedCatalog = (Catalog)serializator.Deserialize(fs);
            }

            Assert.IsTrue(File.Exists(fileName));
            Assert.IsNotNull(deserializedCatalog);
            Assert.IsTrue(deserializedCatalog.Books.Count == 1);
        }
    }
}
