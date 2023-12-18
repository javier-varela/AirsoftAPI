using AirsoftAPI.Models.Dtos;
using AirsoftAPI.Models;
using AirsoftAPI.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AirsoftAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoRepository _repositoryProductos;
        private readonly IMapper _mapper;
        protected ApiResponse _response;
        public ProductosController(IProductoRepository repositoryProductos, IMapper mapper)
        {
            _mapper = mapper;
            _repositoryProductos = repositoryProductos;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse>> GetProductos()
        {
            try
            {
                IEnumerable<Producto> listaProductos = await _repositoryProductos.GetAll(includeProperties:"Categoria");
                _response.Result = _mapper.Map<IEnumerable<ProductoDTO>>(listaProductos);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.ErrorMessages.Add(ex.ToString());
            }
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.InternalServerError;
            return _response;
            
        }

        [HttpGet("{id:int}", Name = "GetProducto")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> GetProducto(int id)
        {
            try
            {
                if(id == 0)
                {
                    _response.StatusCode=HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var producto = await _repositoryProductos.GetFirstOrDefault(p => p.Id == id, includeProperties: "Categoria");

                if (producto == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<ProductoDTO>(producto);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch(Exception ex)
            {
                _response.ErrorMessages.Add(ex.ToString());
            }
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.InternalServerError;
            return _response;
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(CategoriaDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> CrearProducto([FromBody] CrearProductoDTO crearProductoDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (crearProductoDTO == null)
                {
                    return BadRequest(crearProductoDTO);
                }

                if (await _repositoryProductos.Exists(p => p.Nombre.ToLower().Trim() == crearProductoDTO.Nombre.ToLower().Trim()))
                {
                    ModelState.AddModelError("Nombre", "El producto ya existe");
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState);
                }

                var producto = _mapper.Map<Producto>(crearProductoDTO);

                if (!await _repositoryProductos.Add(producto))
                {
                    throw new Exception("Error creando el producto");
                }
                _response.StatusCode = HttpStatusCode.Created;
                _response.Result = producto;
                return CreatedAtRoute("GetProduucto", new { id = producto.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.ErrorMessages.Add(ex.ToString());
            }
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.InternalServerError;
            return StatusCode(500,_response);

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
            if (!await _repositoryProductos.Exists(p=>p.Id==id))
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
