using AirsoftAPI.Models;
using AirsoftAPI.Models.Dtos;
using AirsoftAPI.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Net;
using XAct;

namespace AirsoftAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservasController : ControllerBase
    {
        private readonly ICompraRepository _repositoryCompras;
        private readonly IMapper _mapper;
        protected ApiResponse _response;
        public ReservasController(ICompraRepository compraRepository, IMapper mapper)
        {
            _mapper = mapper;
            _repositoryCompras = compraRepository;
            _response = new();
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse>> GetCompras(int IdUsuario = 0)
        {
            try
            {


                // Obtener compras
                List<Compra> listaCompras = await _repositoryCompras.GetAll(c => c.IdUsuario == IdUsuario, includeProperties: "Producto");


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

        

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
                _response.Result = crearCompraDTO;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.ErrorMessages.Add(ex.Message);
            }
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.InternalServerError;
            return StatusCode(500, _response);

        }
    }
}
