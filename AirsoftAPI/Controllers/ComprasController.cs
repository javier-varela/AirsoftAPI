using AirsoftAPI.Models;
using AirsoftAPI.Models.Dtos;
using AirsoftAPI.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Net;
using XAct;

namespace AirsoftAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComprasController : ControllerBase
    {
        private readonly ICompraRepository _repositoryCompras;
        private readonly IMapper _mapper;
        protected ApiResponse _response;
        public ComprasController(ICompraRepository compraRepository, IMapper mapper)
        {
            _mapper = mapper;
            _repositoryCompras = compraRepository;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse>> GetCompras(int page = 1, int pageSize = 10, 
                                                                int IdUsuario = 0, int IdProducto = 0)
        {
            try
            {
                int skip = (page - 1) * pageSize;

                Expression<Func<Compra, bool>> filter = null;

                if (IdUsuario != 0)
                {
                    filter = c => c.IdUsuario == IdUsuario;
                }

                if (IdProducto != 0)
                {
                    filter = c => c.IdProducto == IdProducto;
                }

                if (IdProducto !=0 && IdUsuario!=0)
                {
                    filter = c => c.IdProducto == IdProducto 
                                && c.IdUsuario == IdUsuario;
                }

                // Obtener compras
                List<Compra> listaCompras = await _repositoryCompras.GetAll(
                    orderBy: q => q.OrderBy(p => p.Id),
                    skip: skip,
                    take: pageSize,
                    filter: filter,
                    includeProperties: "Producto"
                );


                _response.Result = _mapper.Map<IEnumerable<CompraDTO>>(listaCompras);
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

        [HttpGet("{id:int}", Name = "GetCompra")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> GetCompra(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                //Obtener Compra
                var compra = await _repositoryCompras.GetFirstOrDefault(p => p.Id == id, includeProperties: "Producto");

                if (compra == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }


                _response.Result = _mapper.Map<CompraDTO>(compra);

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


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> AgregarCompra(CrearCompraDTO crearCompraDTO)
        {
            try
            {
                if (!ModelState.IsValid || crearCompraDTO == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Result = ModelState;
                    return BadRequest(_response);
                }

                //Agregar Compra
                var compra = _mapper.Map<Compra>(crearCompraDTO);
                compra.Fecha = DateTime.Now;
                if(!await _repositoryCompras.ComprarProducto(compra))
                {
                    throw new Exception("Error guardando la compra");
                }
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetCompra", new { id = compra.Id },_response);
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
