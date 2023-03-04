using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using L01_2020MV603.Models;
using Microsoft.Identity.Client;

namespace L01_2020MV603.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class usuariosController : ControllerBase
    {
        private readonly infoContext _infoContexto;

        public usuariosController(infoContext infoContexto)
        {
            _infoContexto = infoContexto;
        }
        // CREACIÓN DEL CRUD

        // Método para consultar todos los registros de una tabla
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<usuarios> usuarioBlogList = (from u in _infoContexto.usuarios
                                                select u).ToList();
            if (usuarioBlogList.Count == 0)
            {
                return NotFound();
            }
            return Ok(usuarioBlogList);
        }
        // Método para crear nuevos registros
        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarRegistro([FromBody] usuarios usuarioBlogList)
        {
            try
            {
                _infoContexto.usuarios.Add(usuarioBlogList);
                _infoContexto.SaveChanges();
                return Ok(usuarioBlogList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //Actualizar un registro median el parametro del ID.
        [HttpPut]
        [Route("actualizar/id")]
        public IActionResult ActualizarRegistro(int id, [FromBody] usuarios modificarUsuarios) 
        {
            //Para actualizar un registro, obtenemos el original desde la base de datos
            //al cual se le alterara una propiedad
            usuarios? usuariosActuales = (from u in _infoContexto.usuarios
                                                      where u.usuarioId == id
                                                      select u).FirstOrDefault();
            //Verificación de existencia del registro
            if(usuariosActuales == null) 
            { return NotFound(); }

            //Si se encuentra el registro, se alteran los campos
            usuariosActuales.usuarioId = modificarUsuarios.usuarioId;
            usuariosActuales.rolId = modificarUsuarios.rolId;
            usuariosActuales.nombreUsuario = modificarUsuarios.nombreUsuario;
            usuariosActuales.clave = modificarUsuarios.clave;
            usuariosActuales.nombre = modificarUsuarios.nombre;
            usuariosActuales.apellido = modificarUsuarios.apellido;

            //Se marca el registro como modificado
            //Luego se envia la modificacion a la base de datos
            _infoContexto.Entry(usuariosActuales).State = EntityState.Modified;
            _infoContexto.SaveChanges();

            return Ok();
        }
        //Eliminar mediante el ID
        [HttpDelete]
        [Route("eliminar/id")]
        public IActionResult EliminarUsuario(int id)
        {
                usuarios? usuario = (from u in _infoContexto.usuarios
                                     where u.usuarioId==id
                                     select u).FirstOrDefault();
            if (usuario == null)
                return NotFound();
            _infoContexto.usuarios.Attach(usuario);
            _infoContexto.usuarios.Remove(usuario);
            _infoContexto.SaveChanges();

            return Ok();    
        }
        // Método para mostrar los registro mediante nombre o apellido
        // filtro por nombre y apellido
        [HttpGet]
        [Route("find/(filtro)")]
        public IActionResult findbynameandlastname(string filtro)
        {
            usuarios? usuario = (from e in _infoContexto.usuarios
                                 where e.nombre.Contains(filtro)
                                || e.apellido.Contains(filtro)
                                 select e).FirstOrDefault();

            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }
        //Método para mostrar los registro mediante Rol
        //filtrar por rol
        [HttpGet]
        [Route("find/{filtroRolId}")]
        public IActionResult findbyrol(int filtroRolId)
        {
            usuarios? usuario = (from e in _infoContexto.usuarios
                                 where e.rolId == filtroRolId
                                 select e).FirstOrDefault();

            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }
    }
}
