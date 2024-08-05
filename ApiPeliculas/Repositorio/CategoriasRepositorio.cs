﻿using ApiPeliculas.Data;
using ApiPeliculas.Modelos;
using ApiPeliculas.Repositorio.IRepositorio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiPeliculas.Repositorio
{
    public class CategoriasRepositorio : ICategoriasRepositorio
    {
        private readonly ApplicationDbContext _bd;

        public CategoriasRepositorio(ApplicationDbContext db)
        {
            _bd = db;
        }

        public bool ActualizarCategoria(Categorias categoria)
        {
            categoria.FechaCreacion = DateTime.Now;
            _bd.Categorias.Update(categoria);

            return Guardar();
        }

        public bool BorrarCategoria(Categorias categoria)
        {
            _bd.Categorias.Remove(categoria);

            return Guardar();
        }
        public bool CrearCategoria(Categorias categoria)
        {
            categoria.FechaCreacion = DateTime.Now;
            _bd.Categorias.Add(categoria);

            return Guardar();
        }

        public bool ExisteCategoria(int CategoriaId)
        {
            return _bd.Categorias.Any(c => c.idCategoria == CategoriaId);
        }

        public bool ExisteCategoria(string nombreCategoria)
        {
            bool valor = _bd.Categorias.Any(c => c.NombreCategoria.ToLower().Trim() == nombreCategoria.ToLower().Trim());
            return valor;
        }

        public ICollection<Categorias> GetCategorias()
        {
            return _bd.Categorias.OrderBy(c => c.NombreCategoria).ToList();
        }

        public Categorias GetCategorias(int CategoriaId)
        {
            return _bd.Categorias.FirstOrDefault(c => c.idCategoria == CategoriaId);
        }

        public bool Guardar()
        {
            return _bd.SaveChanges() >= 0 ? true : false;
        }
    }
}
