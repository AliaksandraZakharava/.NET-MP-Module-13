using System;
using System.Runtime.Serialization;

namespace Task.DB
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [DataContract(Name = "category", Namespace = "")]
    public class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        [DataMember(Name = "id")]
        public int CategoryID { get; set; }

        [Required]
        [StringLength(15)]
        [DataMember(Name = "title")]
        public string CategoryName { get; set; }

        [Column(TypeName = "ntext")]
        [DataMember(Name = "description")]
        public string Description { get; set; }

        [Column(TypeName = "image")]
        [DataMember(Name = "image_bytes")]
        public byte[] Picture { get; set; }

        [DataMember(Name = "product")]
        public virtual HashSet<Product> Products { get; set; }

        #region Serialization methods

        [OnSerialized]
        void OnSerialized(StreamingContext ctx)
        {
            Console.WriteLine($"Category {CategoryName} has been serialized");
        }

        [OnDeserialized]
        void OnDeserialized(StreamingContext ctx)
        {
            Console.WriteLine($"Category {CategoryName} has been deserialized");
        }

        #endregion
    }
}
