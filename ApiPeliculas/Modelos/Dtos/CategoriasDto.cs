using System.ComponentModel.DataAnnotations;
using System;

namespace ApiPeliculas.Modelos.Dtos
{
    /*
     * Tenemos que entender que un DTO es la manera en cómo los datos
     * van a enviarse a través de la red, siendo así una manera de manejar
     * lo que se muestra al usuario los datos, incluso dejando de lado datos 
     * que no se ocupen o no necesite ver el usuario.
     * 
     * En este DTO es para poder mostrarlo en una tabla los datos completos,
     * por lo que colocamos todos los datos, pero mandando una que otra
     * validación.
     */
    public class CategoriasDto
    {
        public int idCategoria { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio!")]
        [MaxLength(100, ErrorMessage = "El máximo de carácteres es de 100!")]
        public string NombreCategoria { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
