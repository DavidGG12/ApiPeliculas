using Microsoft.VisualBasic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPeliculas.Modelos
{
    public class Peliculas
    {
        [Key]
        public int PeliculaID { get; set; }
        [Required]
        public string? NombrePelicula { get; set; }
        [Required]
        public string? Descripcion { get; set; }
        public string RutaImagen { get; set; }
        [Required]
        public int Duracion { get; set; }
        public enum TipoClasificacion { Siete, Trece, Dieciseis, Dieciocho }
        public TipoClasificacion Clasificacion { get; set; }
        public DateTime FechaCreacion { get; set; }

        //Relación con categoría
        public int categoriaID { get; set; }
        [ForeignKey("idCategoria")]
        public Categorias Categorias { get; set; }
    }
}
