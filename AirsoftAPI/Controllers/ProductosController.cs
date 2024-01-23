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
        public async Task<ActionResult<ApiResponse>> GetProductos(int page = 1, int pageSize = 10)
        {
            try
            {
                int skip = (page - 1) * pageSize;

                //obtener productos
                List<Producto> listaProductos = await _repositoryProductos.GetAll(
                    includeProperties: "Categoria,Imagenes",
                    orderBy: q => q.OrderBy(p => p.Id),
                    skip: skip,
                    take: pageSize
                );

                
                _response.Result = _mapper.Map<IEnumerable<ProductoDTO>>(listaProductos);
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

                //Obtener Producto
                var producto = await _repositoryProductos.GetFirstOrDefault(p => p.Id == id, includeProperties: "Categoria,Imagenes");

                if (producto == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }

                
                _response.Result = _mapper.Map<ProductoDTO>(producto);
             
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> CrearProducto([FromForm] CrearProductoDTO crearProductoDTO)
        {
            try
            {
                if (!ModelState.IsValid || crearProductoDTO == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode=HttpStatusCode.BadRequest;
                    _response.Result = ModelState;
                    return BadRequest(_response);
                }

                if (await _repositoryProductos.Exists(p => p.Nombre.ToLower().Trim() == crearProductoDTO.Nombre.ToLower().Trim()))
                {
                    _response.StatusCode=HttpStatusCode.BadRequest;
                    _response.IsSuccess=false;
                    ModelState.AddModelError("Nombre", "El producto ya existe");
                    _response.Result = ModelState;
                    return BadRequest(_response);
                }
                if (!await _repositoryProductos.AnyCategoriaAsync(crearProductoDTO.CategoriaId))
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    ModelState.AddModelError("CategoriaId", "La Categoria no existe");
                    _response.Result = ModelState;
                    return BadRequest(_response);
                }

                //Agregar Producto
                var producto = await _repositoryProductos.AddProducto(crearProductoDTO);
                

                _response.StatusCode = HttpStatusCode.Created;
                _response.Result = _mapper.Map<ProductoDTO>(producto);

                

                return CreatedAtRoute("GetProducto", new {id= producto.Id}, _response);
            }
            catch (Exception ex)
            {
                _response.ErrorMessages.Add(ex.ToString());
            }
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.InternalServerError;
            return StatusCode(500,_response);

        }


        [HttpPatch("{id:int}", Name = "UpsertProducto")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpsertProducto(int id,[FromForm] UpdateProductoDTO updateProductoDTO)
        {

            try
            {
                
                var actualProduct = await _repositoryProductos.Get(id);
                if (!ModelState.IsValid || updateProductoDTO == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Result = ModelState;
                    return BadRequest(_response);
                }
                if (actualProduct == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                if (actualProduct.Nombre.ToLower().Trim() == updateProductoDTO.Nombre.ToLower().Trim()
                    && updateProductoDTO.Nombre != actualProduct.Nombre)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    ModelState.AddModelError("Nombre", "El producto ya existe");
                    _response.Result = ModelState;
                    return BadRequest(_response);
                } 
                
                if (!await _repositoryProductos.Update(updateProductoDTO))
                {
                    throw new Exception("Error actualizando el producto");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _response.ErrorMessages.Add(ex.ToString());
            }
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.InternalServerError;
            return StatusCode(500, _response);


        }

        [HttpDelete("{id:int}", Name = "DeleteProducto")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            try
            {
                if (!await _repositoryProductos.Exists(p => p.Id == id))
                {
                    _response.IsSuccess = false;
                    _response.StatusCode=HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                var producto = await _repositoryProductos.GetFirstOrDefault(p=>p.Id==id,includeProperties:"Imagenes");

                if (!await _repositoryProductos.Remove(producto))
                {
                    throw new Exception("Error borrando el producto");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _response.ErrorMessages.Add(ex.ToString());
            }
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.InternalServerError;
            return StatusCode(500, _response);
            
        }


    }
}
