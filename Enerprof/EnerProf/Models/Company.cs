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
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Img { get; set; }
        [Column(TypeName = "ntext")]
        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string Description { get; set; }
    }
}