﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models
{
    public class Articulo
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Ingrese el nombre del articulo")]
        [Display(Name = "Nombre del Articulo")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Ingrese la descripcion")]
        [Display(Name = "Descripcion")]
        public string Descripcion { get; set; }
        [Display(Name = "Fecha de creacion")]
        public string FechaCreacion { get; set; }
        [DataType(DataType.ImageUrl)]
        [Display(Name = "Imagen")]
        public string UrlImagen { get; set; }
        [Required]
        public int CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public Categoria Categoria { get; set; }
    }
}
