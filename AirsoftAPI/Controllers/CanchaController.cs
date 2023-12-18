using AirsoftAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CanchaController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Cancha> GetCanchas()
        {
            return new List<Cancha>
            {
                new() {Id=1, Nombre = "Cancha 1"},
                new() {Id=2, Nombre = "Cancha 2"}
            };
        }
    }
}
