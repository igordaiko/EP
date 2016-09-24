using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
namespace EnerProf.Models
{
    public class Product
    {
        public int Id { get; set; }
        public Company Company { get; set; }
        [Display(Name = "Категория")]
        public Category Category { get; set; }
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Изображение")]
        public string Img { get; set; }
        [Column(TypeName = "ntext")]
        [Display(Name = "Описание")]
        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string Description { get; set; }
        public string Images { get; set; }
        public string Files { get; set; }
        [Display(Name = "Видео")]
        public string Video { get; set; }

        public virtual ICollection<Product> SameProducts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Model> Models { get; set; }

        [NotMapped]
        public PagesInfo PagesInfo { get; set; }
    }

    public class Model
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Product Product { get; set; }
        public virtual ICollection<AttributesValues> Properties { get; set; }
    }
}
