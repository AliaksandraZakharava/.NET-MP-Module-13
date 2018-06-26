using System;
using System.Xml.Serialization;

namespace NETMP.Module13.Serialization.Basic
{
    [Serializable]
    public enum Genre
    {
        [XmlEnum(Name = "Not set")]
        NotSet,

        Fantasy,

        Computer,

        Romance,

        Horror,

        [XmlEnum(Name= "Science Fiction")]
        ScienceFiction
    }
}
