using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
namespace EnerProf.Models
{
    [NotMapped]
    public class PagesInfo
    {
        // Кол-во товаров
        public int TotalItems { get; set; }

        // Кол-во товаров на одной странице
        public int ItemsPerPage { get; set; }

        // Номер текущей страницы
        public int CurrentPage { get; set; }

        // Общее кол-во страниц
        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage); }
        }
    }

}