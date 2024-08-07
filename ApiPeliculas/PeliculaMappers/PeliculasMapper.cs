using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dtos;
using AutoMapper;

namespace ApiPeliculas.PeliculaMapper
{
    /*
     * Este mapeado, lo que va a hacer es simplemente hacer 
     * que se comuniquen los Dtos con la clase que construimos
     * con los datos llamada "Categorias"
     */
    public class PeliculasMapper : Profile
    {
        public PeliculasMapper()
        {
            //Mappers de Categorías
            CreateMap<Categorias, CategoriasDto>().ReverseMap();
            CreateMap<Categorias, CrearCategoriasDto>().ReverseMap();
            CreateMap<Categorias, EliminarCategoriasDto>().ReverseMap();
            
            //Mappers de Peliculas
            CreateMap<Peliculas, PeliculasDto>().ReverseMap();
            CreateMap<Peliculas, CrearPeliculasDto>().ReverseMap();
            CreateMap<Peliculas, EliminarPeliculasDto>().ReverseMap();
        }
    }
}
