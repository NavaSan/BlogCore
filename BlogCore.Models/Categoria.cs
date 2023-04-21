using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Escriba la categoria")]
        [Display(Name = "Nombre Categoría")]
        public string Nombre { get; set; }
        [Display(Name = "Orden de Visualizacion")]
        public int? Orden { get; set; }
    }
}
