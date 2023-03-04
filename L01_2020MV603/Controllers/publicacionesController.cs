using L01_2020MV603.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using L01_2020MV603.Models;

namespace L01_2020MV603.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class publicacionesController : ControllerBase
    {
        private readonly infoContext _infoContexto;

        public publicacionesController(infoContext infoContexto)
        {
            _infoContexto = infoContexto;
        }
        // CREACIÓN DEL CRUD

        // Método para consultar todos los registros de una tabla
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<publicaciones> publicacionesBlogList = (from p in _infoContexto.publicaciones
                                                    select p).ToList();
            if (publicacionesBlogList.Count == 0)
            {
                return NotFound();
            }
            return Ok(publicacionesBlogList);
        }
        // Método para crear nuevos registros
        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarRegistro([FromBody] publicaciones publicacionBlogList)
        {
            try
            {
                _infoContexto.publicaciones.Add(publicacionBlogList);
                _infoContexto.SaveChanges();
                return Ok(publicacionBlogList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //Actualizar un registro median el parametro del ID.
        [HttpPut]
        [Route("actualizar/id")]
        public IActionResult ActualizarRegistro(int id, [FromBody] publicaciones modificarPublicaciones)
        {
            //Para actualizar un registro, obtenemos el original desde la base de datos
            //al cual se le alterara una propiedad
            publicaciones? publicacionesActuales = (from u in _infoContexto.publicaciones
                                          where u.publicacionId == id
                                          select u).FirstOrDefault();
            //Verificación de existencia del registro
            if (publicacionesActuales == null)
            { return NotFound(); }

            //Si se encuentra el registro, se alteran los campos
            publicacionesActuales.usuarioId = modificarPublicaciones.usuarioId;
            publicacionesActuales.titulo = modificarPublicaciones.titulo;
            publicacionesActuales.descripcion = modificarPublicaciones.descripcion;

            //Se marca el registro como modificado
            //Luego se envia la modificacion a la base de datos
            _infoContexto.Entry(publicacionesActuales).State = EntityState.Modified;
            _infoContexto.SaveChanges();

            return Ok();
        }
        //Eliminar mediante el ID
        [HttpDelete]
        [Route("eliminar/id")]
        public IActionResult EliminarUsuario(int id)
        {
            publicaciones? publicacion = (from u in _infoContexto.publicaciones
                                 where u.publicacionId == id
                                 select u).FirstOrDefault();
            if (publicacion == null)
                return NotFound();
            _infoContexto.publicaciones.Attach(publicacion);
            _infoContexto.publicaciones.Remove(publicacion);
            _infoContexto.SaveChanges();

            return Ok();
        }
        //Método para mostrar los registro mediante Usuario
        //filtrar por Usuario
        [HttpGet]
        [Route("find/{filtroUsuarioId}")]
        public IActionResult findbyrol(int filtroUsuarioId)
        {
            publicaciones? publicacion = (from e in _infoContexto.publicaciones
                                 where e.usuarioId == filtroUsuarioId
                                 select e).FirstOrDefault();

            if (publicacion == null)
            {
                return NotFound();
            }
            return Ok(publicacion);
        }
    }
}
