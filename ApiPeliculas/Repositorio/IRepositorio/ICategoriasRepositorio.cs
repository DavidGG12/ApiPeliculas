using ApiPeliculas.Modelos;
using System.Collections.Generic;

namespace ApiPeliculas.Repositorio.IRepositorio
{
    public interface ICategoriasRepositorio
    {
        //Esta interface es la estructura que va a tener la clase con la que vamos a trabajar, en este caso, con el repositorio
        //Tenemos las funciones con las que vamos a hacer un CRUD
        ICollection<Categorias>GetCategorias();
        Categorias GetCategorias(int CategoriaId);
        bool ExisteCategoria(int CategoriaId);
        bool ExisteCategoria(string nombreCategoria);
        bool BorrarCategoria(Categorias categorias);
        bool CrearCategoria(Categorias categoria);
        bool ActualizarCategoria(Categorias categoria);
        bool Guardar();
    }
}
