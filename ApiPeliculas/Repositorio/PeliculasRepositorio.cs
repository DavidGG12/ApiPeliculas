using ApiPeliculas.Data;
using ApiPeliculas.Modelos;
using ApiPeliculas.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiPeliculas.Repositorio
{
    public class PeliculasRepositorio : IPeliculasRepositorio
    {
        private readonly ApplicationDbContext _bd;

        public PeliculasRepositorio(ApplicationDbContext db)
        {
            _bd = db;
        }

        public bool ActualizarPelicula(Peliculas peliculas)
        {
            peliculas.FechaCreacion = DateTime.Now;

            //Arreglar problema del PUT
            var peliculaExistente = _bd.Peliculas.Find(peliculas.PeliculaID);

            if (peliculaExistente != null) _bd.Entry(peliculaExistente).CurrentValues.SetValues(peliculas);
            else _bd.Peliculas.Update(peliculas);

            return Guardar();
        }

        public bool BorrarPelicula(Peliculas peliculas)
        {
            _bd.Peliculas.Remove(peliculas);

            return Guardar();
        }

        public IEnumerable<Peliculas> BuscarPelicula(string nombrePelicula)
        {
            IQueryable<Peliculas> query = _bd.Peliculas;
            if (!string.IsNullOrEmpty(nombrePelicula)) query = query.Where(e => e.NombrePelicula.Contains(nombrePelicula) || e.Descripcion.Contains(nombrePelicula));
            return query.ToList();
        }

        public bool CrearPelicula(Peliculas peliculas)
        {
            peliculas.FechaCreacion = DateTime.Now;
            _bd.Peliculas.Add(peliculas);

            return Guardar();
        }

        public bool ExistePelicula(int id)
        {
            return _bd.Peliculas.Any(c => c.PeliculaID == id);
        }

        public bool ExistePelicula(string nombrePelicula)
        {
            return _bd.Peliculas.Any(c => c.NombrePelicula.ToLower().Trim() == nombrePelicula.ToLower().Trim());
        }

        public ICollection<Peliculas> GetPeliculas()
        {
            return _bd.Peliculas.OrderBy(c => c.NombrePelicula).ToList();
        }

        public Peliculas GetPeliculas(int peliculaID)
        {
            return _bd.Peliculas.FirstOrDefault(c => c.PeliculaID == peliculaID);
        }

        public ICollection<Peliculas> GetPeliculasEnCategoria(int catID)
        {
            return _bd.Peliculas.Include(ca => ca.Categorias).Where(ca => ca.categoriaID == catID).ToList();
        }

        public bool Guardar()
        {
            return _bd.SaveChanges() >= 0 ? true : false;
        }
    }
}
