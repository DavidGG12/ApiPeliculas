using ApiPeliculas.Modelos;
using System.Collections.Generic;

namespace ApiPeliculas.Repositorio.IRepositorio
{
    public interface IPeliculasRepositorio
    {
        //Esta interface es la estructura que va a tener la clase con la que vamos a trabajar, en este caso, con el repositorio
        //Tenemos las funciones con las que vamos a hacer un CRUD
        ICollection<Peliculas>GetPeliculas();
        ICollection<Peliculas> GetPeliculasEnCategoria(int catID);
        IEnumerable<Peliculas> BuscarPelicula(string nombrePelicula);
        Peliculas GetPeliculas(int peliculaID);
        bool ExistePelicula(int id);
        bool ExistePelicula(string nombrePelicula);
        bool CrearPelicula(Peliculas peliculas);
        bool BorrarPelicula(Peliculas peliculas);
        bool ActualizarPelicula(Peliculas peliculas);
        bool Guardar();
    }
}
