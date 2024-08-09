using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dtos;
using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

//Este controlador va a ser donde colocaremos los métodos que necesitemos y su lógica: GET, POST, etc.
namespace ApiPeliculas.Controllers
{
    //Es la ruta con la que podremos acceder a nuestra API
    [Route("api/peliculas")]
    [ApiController]
    public class PeliculasController : ControllerBase
    {
        private readonly IPeliculasRepositorio _plRepo;
        private readonly ICategoriasRepositorio _ctRepo;
        private readonly IMapper _mapper;

        public PeliculasController(IPeliculasRepositorio plRepo, ICategoriasRepositorio ctRepo, IMapper mapper)
        {
            _plRepo = plRepo;
            _ctRepo = ctRepo;
            _mapper = mapper;
        }

        /*
         * Específicamos el tipo de método que vamos a hacer,
         * en este caso es un método GET donde puede arrojar 
         * el estatus 403 y el estatus 200.
         */
        #region Método GET
        [HttpGet("all")]
        [ResponseCache(Duration = 30)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetPeliculas()
        { 
            var listaPeliculas = _plRepo.GetPeliculas();
            var listaPeliculasDto = new List<PeliculasDto>();

            foreach (var lista in listaPeliculas)
            {
                listaPeliculasDto.Add(_mapper.Map<PeliculasDto>(lista));
            }

            return Ok(listaPeliculasDto);
        }
        #endregion

        #region Método GET {id}
        [HttpGet("id={peliculaID:int}", Name = "GetPeliculas")]
        [ResponseCache(Duration = 30)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPeliculas(int peliculaID)
        {
            var itemPeliculas = _plRepo.GetPeliculas(peliculaID);
            
            if(itemPeliculas == null) return NotFound();

            var itemPeliculasDto = _mapper.Map<PeliculasDto>(itemPeliculas);

            return Ok(itemPeliculasDto);
        }
        #endregion
        
        #region Método POST
        [HttpPost]
        [Authorize(Roles = "admin")]
        //[ProducesResponseType(201, Type = typeof(PeliculasDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CrearPelicula([FromBody] CrearPeliculasDto crearPeliculasDto)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            if(crearPeliculasDto == null) return BadRequest(ModelState);

            if (_plRepo.ExistePelicula(crearPeliculasDto.NombrePelicula))
            {
                ModelState.AddModelError("", $"La película ya existe");
                return StatusCode(404, ModelState);
            }

            if(!_ctRepo.ExisteCategoria(crearPeliculasDto.categoriaID))
            {
                ModelState.AddModelError("", $"El id es incorrecto");
                return StatusCode(404, ModelState);
            }

            var pelicula = _mapper.Map<Peliculas>(crearPeliculasDto);

            if(!_plRepo.CrearPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salió mal en el registro {pelicula.NombrePelicula}");
                return StatusCode(404, ModelState);
            }
            //ModelState.AddModelError("", $"El id de categoria es: {pelicula.categoriaID}");

            //return StatusCode(404, ModelState); 
            return CreatedAtRoute("GetPelicula", new { peliculaID = pelicula.PeliculaID }, pelicula);
        }
        #endregion

        #region Método PATCH
        [Authorize(Roles = "admin")]
        [HttpPatch("{peliculaID:int}", Name = "ActualizarPelicula")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult ActualizarPelicula(int peliculaID, [FromBody] PeliculasDto peliculasDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (peliculasDto == null || peliculaID != peliculasDto.PeliculaID) return BadRequest(ModelState);

            var peliculaExistente = _plRepo.GetPeliculas(peliculaID);
            if (peliculaExistente == null) return NotFound($"No se encontró la pelicula con el ID {peliculaID}");

            var pelicula = _mapper.Map<Peliculas>(peliculasDto);
            
            if (!_plRepo.ActualizarPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salió mal en la actualización {pelicula.NombrePelicula}");
                return StatusCode(404, ModelState);
            }

            return NoContent();
        }
        #endregion

        #region Metodo DELETE
        [Authorize(Roles = "admin")]
        [HttpDelete("{peliculaID:int}", Name = "BorrarPelicula")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult BorrarPelicula(int peliculaID)
        {
            if (!_plRepo.ExistePelicula(peliculaID)) return NotFound();

            var pelicula = _plRepo.GetPeliculas(peliculaID);

            if(!_plRepo.BorrarPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salió mal con el borrado del registro: {pelicula.NombrePelicula}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
        #endregion

        #region Método Búsqueda de Películas
        [HttpGet("pelicula_categoria={categoriaID:int}")]
        [ResponseCache(Duration = 30)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetPeliculasEnCategoria(int categoriaID)
        {
            var listaPeliculas = _plRepo.GetPeliculasEnCategoria(categoriaID);
            if(listaPeliculas == null) return NotFound();

            var itemPeliculas = new List<PeliculasDto>();
            foreach(var pelicula in listaPeliculas)
            {
                itemPeliculas.Add(_mapper.Map<PeliculasDto>(pelicula));
            }
            return Ok(itemPeliculas);
        }
        #endregion

        #region Método Búsqueda
        [HttpGet("search/", Name = "Buscar")]
        [ResponseCache(Duration = 30)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Buscar(string busqueda)
        {
            try
            {
                var resultado = _plRepo.BuscarPelicula(busqueda);
                if(resultado.Any()) return Ok(resultado);

                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos");
            }
        }
        #endregion
    }
}
