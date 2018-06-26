using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace NETMP.Module13.Serialization.Basic
{
    [Serializable]
    [XmlRoot("catalog", Namespace = "http://library.by/catalog")]
    public class Catalog
    {
        [XmlAttribute("date")]
        public DateTime Date { get; set; }

        [XmlElement("book")]
        public List<Book> Books { get; set; }
    }
}
