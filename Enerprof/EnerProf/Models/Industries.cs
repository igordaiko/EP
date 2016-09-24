using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
namespace EnerProf.Models
{
    public class Industries
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Img { get; set; }
        [Column(TypeName = "ntext")]
        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string Description { get; set; }

        [Display(Name = "Интересные товары")]
        public virtual List<Product> Products { get; set; }
    }
}