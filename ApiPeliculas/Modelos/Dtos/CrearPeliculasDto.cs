using System.ComponentModel.DataAnnotations;
using System;

namespace ApiPeliculas.Modelos.Dtos
{
    /*
     * En este otro ejemplo, para crear la categoría solo necesitamos
     * el nombre, por lo que no es necesario traer el id o la fecha, 
     * así que quitamos todo lo demás.
     */
    public class CrearPeliculasDto
    {
        public string NombrePelicula { get; set; }
        public string Descripcion { get; set; }
        public string RutaImagen { get; set; }
        public int Duracion { get; set; }
        public enum TipoClasificacion { Siete, Trece, Dieciseis, Dieciocho }
        public TipoClasificacion Clasificacion { get; set; }
        public int categoriaID { get; set; }
        public int idCategoria { get; set; }
    }
}
