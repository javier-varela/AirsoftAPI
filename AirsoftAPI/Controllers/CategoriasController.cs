using AirsoftAPI.Models;
using AirsoftAPI.Models.Dtos;
using AirsoftAPI.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AirsoftAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaRepository _repositoryCategoria;
        private readonly IMapper _mapper;

        public CategoriasController(ICategoriaRepository repositoryCategoria, IMapper mapper)
        {
            _mapper = mapper;
            _repositoryCategoria = repositoryCategoria;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategorias()
        {
            IEnumerable<Categoria> listaCategorias = await _repositoryCategoria.GetAll();

            return Ok(_mapper.Map<IEnumerable<CategoriaDTO>>(listaCategorias));
        }

        [HttpGet("{id:int}", Name ="GetCategoria")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCategoria(int id)
        {

            var categoria = await _repositoryCategoria.Get(id);

            if (categoria == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<CategoriaDTO>(categoria));
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(CategoriaDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CrearCategoria([FromBody] CrearCategoriaDTO crearCategoriaDTO)
        {
            if (!ModelState.IsValid || crearCategoriaDTO == null)
            {
                return BadRequest(ModelState);
            }

            if (await _repositoryCategoria.Exists(c => c.Nombre.ToLower().Trim() == crearCategoriaDTO.Nombre.ToLower().Trim()))
            {
                ModelState.AddModelError("", "La categoria con ese nombre ya existe");
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            var categoria = _mapper.Map<Categoria>(crearCategoriaDTO);

            if (!await _repositoryCategoria.Add(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal guardando la categoria {categoria.Nombre}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCategoria", new { id = categoria.Id }, categoria);
        }


        [HttpPatch("{id:int}",Name = "PatchCategoria")]
        [ProducesResponseType(201, Type = typeof(CategoriaDTO))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PatchCategoria(int id, [FromBody] CrearCategoriaDTO crearCategoriaDTO)
        {
            if (!ModelState.IsValid || crearCategoriaDTO == null)
            {
                return BadRequest(ModelState);
            }

            var categoria = _mapper.Map<Categoria>(crearCategoriaDTO);
            categoria.Id = id;

            if ( ! await _repositoryCategoria.Update(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal actualizando la categoria {categoria.Nombre}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "DeleteCategoria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            if (!await _repositoryCategoria.Exists(c=>c.Id==id))
            {
                return NotFound();
            }



            var categoria = await _repositoryCategoria.Get(id);

            if (!await _repositoryCategoria.Remove(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal borrando la categoria {categoria.Nombre}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


    }
}
