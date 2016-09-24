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
    public class Category
    {
        public int Id { get; set; }
        [Display(Name = "Название")]
        [Required(ErrorMessage = "Пожалуйста, введите название")]
        public string Name { get; set; }
        [Display(Name = "Изображение")]
        [MaxLength(255)]
        public string Img { get; set; }
        [Display(Name = "Родительская категория")]
        public Category Parent { get; set; }
        [Column(TypeName = "ntext")]
        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string Description { get; set; }
        [Required]
        public bool IsOnStartPage { get; set; }
        public virtual ICollection<AttributeModel> Attributes { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        [NotMapped]
        public List<Category> Sub { get; set; }
        //[NotMapped]
        //public PagesInfo PagingInfo { get; set; }
    }
}