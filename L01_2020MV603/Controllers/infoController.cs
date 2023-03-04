using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using L01_2020MV603.Models;
using Microsoft.Identity.Client;

namespace L01_2020MV603.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class infoController : ControllerBase
    {
        private readonly infoContext _infoContexto;

        public infoController(infoContext infoContexto) 
        {
            _infoContexto= infoContexto;
        }
        [HttpGet]
        [Route("GetAll")]

        public IActionResult Get() 
        { 
            List<calificaciones> ListadoBlog = (from e in _infoContexto.calificaciones
                                        select e).ToList();
            if (ListadoBlog.Count > 0 ) 
            {
                return NotFound();
            }
            return Ok(ListadoBlog);
        }
    }
}
