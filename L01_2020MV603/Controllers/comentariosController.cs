using L01_2020MV603.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace L01_2020MV603.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class comentariosController : ControllerBase
    {
        private readonly infoContext _infoContexto;

        public comentariosController(infoContext infoContexto)
        {
            _infoContexto = infoContexto;
        }
        // CREACIÓN DEL CRUD

        // Método para consultar todos los registros de una tabla
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<comentarios> comentarioBlogList = (from p in _infoContexto.comentarios
                                                         select p).ToList();
            if (comentarioBlogList.Count == 0)
            {
                return NotFound();
            }
            return Ok(comentarioBlogList);
        }
        // Método para crear nuevos registros
        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarRegistro([FromBody] comentarios comentarioBlogList)
        {
            try
            {
                _infoContexto.comentarios.Add(comentarioBlogList);
                _infoContexto.SaveChanges();
                return Ok(comentarioBlogList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //Actualizar un registro median el parametro del ID.
        [HttpPut]
        [Route("actualizar/id")]
        public IActionResult ActualizarRegistro(int id, [FromBody] comentarios modificarComentarios)
        {
            //Para actualizar un registro, obtenemos el original desde la base de datos
            //al cual se le alterara una propiedad
            comentarios? comentariosActuales = (from u in _infoContexto.comentarios
                                                    where u.cometarioId == id
                                                    select u).FirstOrDefault();
            //Verificación de existencia del registro
            if (comentariosActuales == null)
            { return NotFound(); }

            //Si se encuentra el registro, se alteran los campos
            comentariosActuales.usuarioId = modificarComentarios.usuarioId;
            comentariosActuales.publicacionId = modificarComentarios.publicacionId;
            comentariosActuales.comentario = modificarComentarios.comentario;

            //Se marca el registro como modificado
            //Luego se envia la modificacion a la base de datos
            _infoContexto.Entry(comentariosActuales).State = EntityState.Modified;
            _infoContexto.SaveChanges();

            return Ok();
        }
        //Eliminar mediante el ID
        [HttpDelete]
        [Route("eliminar/id")]
        public IActionResult EliminarUsuario(int id)
        {
            comentarios? comentario = (from u in _infoContexto.comentarios
                                          where u.cometarioId == id
                                          select u).FirstOrDefault();
            if (comentario == null)
                return NotFound();
            _infoContexto.comentarios.Attach(comentario);
            _infoContexto.comentarios.Remove(comentario);
            _infoContexto.SaveChanges();

            return Ok();
        }
        //Método para mostrar los registro mediante el Id de la publicacion
        //filtrar por publicacionId
        [HttpGet]
        [Route("find/{filtroPublicacionId}")]
        public IActionResult findbyrol(int filtroPublicacionId)
        {
            comentarios? comentario = (from e in _infoContexto.comentarios
                                          where e.publicacionId == filtroPublicacionId
                                          select e).FirstOrDefault();

            if (comentario == null)
            {
                return NotFound();
            }
            return Ok(comentario);
        }
    }
}
