using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace EnerProf.Models
{
    public class Clients
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Название компании клиента")]
        [Required(ErrorMessage = "Пожалуйста, введите название")]
        public string Name { get; set; }
        [Display(Name = "Изображение")]
        public string Img { get; set; }
    }
}
