using AirsoftAPI.Models.Dtos;
using AirsoftAPI.Models;
using AirsoftAPI.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoRepository _repositoryProductos;
        private readonly IMapper _mapper;

        public ProductosController(IProductoRepository repositoryProductos, IMapper mapper)
        {
            _mapper = mapper;
            _repositoryProductos = repositoryProductos;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<ProductoDTO>>> GetProductos()
        {
            IEnumerable<Producto> listaProductos = await _repositoryProductos.GetAll();

            return Ok(_mapper.Map<IEnumerable<ProductoDTO>>(listaProductos));
        }

        [HttpGet("{id:int}", Name = "GetProducto")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetProducto(int id)
        {

            var producto = await _repositoryProductos.Get(id);

            if (producto == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<ProductoDTO>(producto));
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(CategoriaDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CrearProducto([FromBody] CrearProductoDTO crearProductoDTO)
        {
            if (!ModelState.IsValid || crearProductoDTO == null)
            {
                return BadRequest(ModelState);
            }

            if (await _repositoryProductos.Exists(crearProductoDTO.Nombre))
            {
                ModelState.AddModelError("", "El producto ya existe");
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            var producto = _mapper.Map<Producto>(crearProductoDTO);

            if (!await _repositoryProductos.Add(producto))
            {
                ModelState.AddModelError("", $"Algo salio mal guardando la categoria {producto.Nombre}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCategoria", new { id = producto.Id }, producto);
        }


        [HttpPatch("{id:int}", Name = "PatchProducto")]
        [ProducesResponseType(201, Type = typeof(CategoriaDTO))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PatchProducto(int id, [FromBody] ProductoDTO productoDTO)
        {
            if (!ModelState.IsValid || productoDTO == null || id != productoDTO.Id)
            {
                return BadRequest(ModelState);
            }



            var producto = _mapper.Map<Producto>(productoDTO);

            if (!await _repositoryProductos.Update(producto))
            {
                ModelState.AddModelError("", $"Algo salio mal actualizando el producto {producto.Nombre}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "DeleteProducto")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            if (!await _repositoryProductos.Exists(id))
            {
                return NotFound();
            }



            var producto = await _repositoryProductos.Get(id);

            if (!await _repositoryProductos.Remove(producto))
            {
                ModelState.AddModelError("", $"Algo salio mal borrando el producto {producto.Nombre}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


    }
}
