using Microsoft.VisualBasic;
using System;
using System.ComponentModel.DataAnnotations;


/*
 * Este es el modelo que vamos a crear para, al momento de hacer la migración y 
 * que pueda crear la tabla en la base de datos.
 */
namespace ApiPeliculas.Modelos
{
    public class Categorias
    {
        //La propiedad [Key] es para decir que va a ser una llave primaria.
        [Key]
        public int idCategoria { get; set; }

        //La propiedad [Required] es para decir que es un valor que no puede ser nulo.
        [Required]
        public string NombreCategoria { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }
    }
}

/*
 * Al terminar de hacer el modelo ahora sí podemos hacer la migración,
 * esto se logra solo con irte a la consola de administración de paquetes
 * y colocar el comando "add-migration" seguido del mensaje que llevará la migración.
 * 
 * Se te creará una nueva carpeta con las migraciones y los archivos con la estructura de la tabla
 * solo confirmas que los datos estén bien y en la misma consola colocamos el 
 * comando "update-database" para poder hacer el push a la base que estemos ocupando.
 */
