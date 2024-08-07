using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace ApiPeliculas.Modelos.Dtos
{
    public class PeliculasDto
    {
        public int PeliculaID { get; set; }
        public string NombrePelicula { get; set; }
        public string Descripcion { get; set; }
        public string RutaImagen { get; set; }
        public int Duracion { get; set; }
        public enum TipoClasificacion { Siete, Trece, Dieciseis, Dieciocho }
        public TipoClasificacion Clasificacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int categoriaID { get; set; }
    }
}
