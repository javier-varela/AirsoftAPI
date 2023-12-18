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
    public class CarritosController:ControllerBase
    {
        private readonly ICarritoRepository _repositoryCarrito;
        private readonly ICompraRepository _repositoryCompra;
        private readonly IMapper _mapper;
        protected ApiResponse _response;
        public CarritosController(ICarritoRepository carritoRepository,ICompraRepository compraRepository, IMapper mapper)
        {
            _mapper = mapper;
            _repositoryCarrito = carritoRepository;
            _repositoryCompra = compraRepository;
            _response = new();
        }
        [HttpGet("{idUsuario:int}", Name = "GetProductosCarrito")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse>> GetProductosCarrito(int idUsuario)
        {
            try
            {
                IEnumerable<ProductoCarrito> listaProductos = await _repositoryCarrito.GetAll
                (
                    (pc)=>pc.IdUsuario == idUsuario,
                    includeProperties: "Producto"
                );
                _response.Result = _mapper.Map<IEnumerable<ProductoCarritoDTO>>(listaProductos);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok( _response );
            }
            catch (Exception ex)
            {
                _response.ErrorMessages.Add(ex.ToString());
            }
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.InternalServerError;
            return _response;

        }

        [HttpGet("{id:int}", Name = "GetProductoCarrito")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> GetProducto(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var productoCarrito = await _repositoryCarrito.GetFirstOrDefault(p => p.Id == id, includeProperties: "Producto");

                if (productoCarrito == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<ProductoCarritoDTO>(productoCarrito);
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

    }
}
