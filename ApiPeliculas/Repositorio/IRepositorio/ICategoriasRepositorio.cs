using ApiPeliculas.Modelos;
using System.Collections.Generic;

namespace ApiPeliculas.Repositorio.IRepositorio
{
    public interface ICategoriasRepositorio
    {
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
