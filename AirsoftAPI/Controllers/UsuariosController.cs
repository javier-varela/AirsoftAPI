using AirsoftAPI.Models.Dtos;
using AirsoftAPI.Models;
using AirsoftAPI.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using AirsoftAPI.Utilities;
using System.Security.Claims;

namespace AirsoftAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioRepository _repositoryUsuario;
        private readonly IMapper _mapper;
        protected ApiResponse _response;

        public UsuariosController(IUsuarioRepository repositoryUsuario, IMapper mapper)
        {
            _mapper = mapper;
            _repositoryUsuario = repositoryUsuario;
            _response = new();
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse>> GetUsuarios()
        {
            try
            {
                
                IEnumerable<Usuario> listaUsuarios = await _repositoryUsuario.GetAll();
                _response.Result = _mapper.Map<IEnumerable<UsuarioDTO>>(listaUsuarios);
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

        //[Authorize]
        [HttpGet("{id:int}", Name = "GetUsuarioById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> GetUsuarioById(int id, bool includeCompras = false)
        {
            try
            {
                
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                //var userId = User.FindFirst(ClaimTypes.Name)?.Value;
                //if (id.ToString() != userId)
                //{
                //    _response.StatusCode = HttpStatusCode.Forbidden;
                //    _response.IsSuccess = false;
                //    return _response;
                //}
                Usuario usuario;
                if (includeCompras)
                {
                    usuario = await _repositoryUsuario.GetFirstOrDefault(u => u.Id == id, includeProperties: "Compras");
                }
                else
                {
                    usuario = await _repositoryUsuario.GetFirstOrDefault(u=>u.Id==id);
                }

                
                if (usuario == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }

               
                _response.Result = _mapper.Map<UsuarioDTO>(usuario);
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

        [HttpPost("Registro")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> Registro([FromBody] UsuarioRegistroDTO usuarioRegistroDTO)
        {
            try
            {
                bool isUnique = await _repositoryUsuario.IsUniqueUser(usuarioRegistroDTO.Nombre);

                if (!isUnique)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("El nombre de usuario ya existe");
                    return BadRequest(_response);
                }
                var usuario = await _repositoryUsuario.Registro(usuarioRegistroDTO);
                if (usuario == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Error en el registro");
                    return BadRequest(_response);
                }
                _response.StatusCode = HttpStatusCode.Created;
                _response.IsSuccess = true;
                _response.Result = usuario;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.ErrorMessages.Add(ex.ToString());
            }
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.InternalServerError;
            return StatusCode(500,_response);


        }
        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> Login([FromBody] UsuarioLoginDTO usuarioLoginDTO)
        {
            try
            {
                var loginResponse = await _repositoryUsuario.Login(usuarioLoginDTO);

                if (loginResponse.Usuario == null || string.IsNullOrEmpty(loginResponse.Token))
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Credenciales Incorrectas");
                    return BadRequest(_response);
                }
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = loginResponse;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.ErrorMessages.Add(ex.ToString());
            }
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.InternalServerError;
            return StatusCode(500,_response);


        }

    }
}
