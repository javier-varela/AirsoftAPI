using AirsoftAPI.Models;
using AirsoftAPI.Models.Dtos;
using AirsoftAPI.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AirsoftAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CanchaController : ControllerBase

    {
        private readonly ICanchaRepository _repositoryCancha;
        private readonly IMapper _mapper;
        protected ApiResponse _response;
        public CanchaController(ICanchaRepository canchaRepository, IMapper mapper)
        {
            _mapper = mapper;
            _repositoryCancha = canchaRepository;
            _response = new();
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse>> GetCanchas(int page = 1, int pageSize = 10)
        {
            try
            {
                int skip = (page - 1) * pageSize;

                //obtener canchas
                _response.Result = await _repositoryCancha.GetAll(
                    includeProperties: "Imagenes",
                    orderBy: q => q.OrderBy(p => p.Id),
                    skip: skip,
                    take: pageSize
                );

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
