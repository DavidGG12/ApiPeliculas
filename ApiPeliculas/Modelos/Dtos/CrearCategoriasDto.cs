using System.ComponentModel.DataAnnotations;
using System;

namespace ApiPeliculas.Modelos.Dtos
{
    /*
     * En este otro ejemplo, para crear la categoría solo necesitamos
     * el nombre, por lo que no es necesario traer el id o la fecha, 
     * así que quitamos todo lo demás.
     */
    public class CrearCategoriasDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio!")]
        [MaxLength(100, ErrorMessage = "El máximo de carácteres es de 100!")]
        public string NombreCategoria { get; set; }
    }
}
