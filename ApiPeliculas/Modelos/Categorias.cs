using Microsoft.VisualBasic;
using System;
using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Modelos
{
    public class Categorias
    {
        [Key]
        public int idCategoria { get; set; }
        [Required]
        public string NombreCategoria { get; set; }
        [Required]
        public DateTime FechaCreacion { get; set; }
    }
}
