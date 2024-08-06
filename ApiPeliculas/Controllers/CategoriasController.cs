using ApiPeliculas.Modelos;
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
        #region Método GET
        [HttpGet("all")]
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
        #endregion

        #region Método GET {id}
        [HttpGet("id={categoriaID:int}", Name = "GetCategorias")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCategorias(int categoriaID)
        {
            var itemCategoria = _ctRepo.GetCategorias(categoriaID);
            
            if(itemCategoria == null) return NotFound();

            var itemCategoriaDto = _mapper.Map<CategoriasDto>(itemCategoria);

            return Ok(itemCategoriaDto);
        }
        #endregion

        #region Método POST
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CrearCategoria([FromBody] CrearCategoriasDto crearCategoriaDto)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            if(crearCategoriaDto == null) return BadRequest(ModelState);

            if (_ctRepo.ExisteCategoria(crearCategoriaDto.NombreCategoria))
            {
                ModelState.AddModelError("", $"La categoría ya existe");
                return StatusCode(404, ModelState);
            }

            var categoria = _mapper.Map<Categorias>(crearCategoriaDto);

            if(!_ctRepo.CrearCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salió mal en el registro {categoria.NombreCategoria}");
                return StatusCode(404, ModelState);
            }

            return CreatedAtRoute("GetCategorias", new { categoriaID = categoria.idCategoria }, categoria);
        }
        #endregion

        #region Método PATCH
        [HttpPatch("{categoriaID:int}", Name = "ActualizarPatchCategoria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult ActualizarPatchCategoria(int categoriaID, [FromBody] CategoriasDto categoriaDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (categoriaDto == null || categoriaID != categoriaDto.idCategoria) return BadRequest(ModelState);

            var categoria = _mapper.Map<Categorias>(categoriaDto);

            if (!_ctRepo.ActualizarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salió mal en la actualización {categoria.NombreCategoria}");
                return StatusCode(404, ModelState);
            }

            return NoContent();
        }
        #endregion

        #region Método PUT
        [HttpPut("{categoriaID:int}", Name = "ActualizarPutCategoria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ActualizarPutCategoria(int categoriaID, [FromBody] CategoriasDto categoriaDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (categoriaDto == null || categoriaID != categoriaDto.idCategoria) return BadRequest(ModelState);

            var categoriaExistente = _ctRepo.GetCategorias(categoriaID);

            if (categoriaExistente == null) return NotFound($"No se encontré la categoría: {categoriaID}");

            var categoria = _mapper.Map<Categorias>(categoriaDto);

            if (!_ctRepo.ActualizarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salió mal en la actualización {categoria.NombreCategoria}");
                return StatusCode(404, ModelState);
            }

            return NoContent();
        }
        #endregion

        #region Metodo DELETE
        [HttpDelete("{categoriaID:int}", Name = "BorrarCategoria")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult BorrarCategoria(int categoriaID)
        {
            if (!_ctRepo.ExisteCategoria(categoriaID)) return NotFound();

            var categoria = _ctRepo.GetCategorias(categoriaID);

            if(!_ctRepo.BorrarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salió mal con el borrado del registro: {categoria.NombreCategoria}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
        #endregion
    }
}
