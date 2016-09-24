using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnerProf.Models
{
    public class Comment
    {
        public int id { get; set; }
        public Product Product { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string Text { get; set; }
        public string Img { get; set; }

    }
}