using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EnerProf.Models
{
    [Table("Attributes")]
    public class AttributeModel
    {
        [Key]
        public int id { get; set; }
        public string Name { get; set; }
        public Types Type { get; set; }
        public Unit Unit { get; set; }
        public bool Status { get; set; }
        public int Ordering { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<AttributesValues> Values { get; set; }
    }

    public class Unit
    {
        [Key]
        public int id { get; set; }
        public string Name { get; set; }
    }

    [Table("Attributes_Values")]
    public class AttributesValues
    {
        public int Id { get; set; }
        public AttributeModel Attribute { get; set; }
        public Model Model { get; set; }
        public double? Double_Value { get; set; }
        public String String_Value { get; set; }

    }
    public enum Types
    {
        Double,
        String
    }
    [NotMapped]
    public class Filters
    {
        public AttributeModel Attribute { get; set; }
        public double? Min_Value { get; set; }
        public double? Max_Value { get; set; }
        public List<string> String_Values { get; set; }
        public List<Product> Products { get; set; }

    }
}