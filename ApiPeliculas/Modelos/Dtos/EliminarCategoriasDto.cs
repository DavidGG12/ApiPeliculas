using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Modelos.Dtos
{
    public class EliminarCategoriasDto
    {
        [Required(ErrorMessage = "El id es obligatorio!")]
        public int categoriaID { get; set; }
    }
}
