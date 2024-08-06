using ApiPeliculas.Modelos.Dtos;
using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

//Este controlador va a ser donde colocaremos los métodos que necesitemos y su lógica: GET, POST, etc.
namespace ApiPeliculas.Controllers
{
    //Es la ruta con la que podremos acceder a nuestra API
    [Route("api/categorias")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriasRepositorio _ctRepo;
        private readonly IMapper _mapper;

        public CategoriasController(ICategoriasRepositorio ctRepo, IMapper mapper)
        {
            _ctRepo = ctRepo;
            _mapper = mapper;
        }

        /*
         * Específicamos el tipo de método que vamos a hacer,
         * en este caso es un método GET donde puede arrojar 
         * el estatus 403 y el estatus 200.
         */
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCategorias()
        {
            /*
             * Iniciamos una variable para obtener los resultados de las categorías
             * y otra para poder ir guardando los datos dentro del DTO que hicimos.
             */
            var listaCategorias = _ctRepo.GetCategorias();
            var listaCategoriasDto = new List<CategoriasDto>();

            foreach (var lista in listaCategorias)
            {
                listaCategoriasDto.Add(_mapper.Map<CategoriasDto>(lista));
            }

            return Ok(listaCategoriasDto);
        }
    }
}
