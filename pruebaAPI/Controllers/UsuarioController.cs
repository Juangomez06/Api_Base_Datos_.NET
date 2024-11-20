using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pruebaAPI.Models;
using pruebaAPI.Servicios;

namespace pruebaAPI.Controllers
{
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        IUsuarioServicio usuarioServicio;

        public UsuarioController( IUsuarioServicio servicio )
        {
            usuarioServicio = servicio;
        }

        [HttpGet]
        [Route("protected")]
        [Authorize]
        public ActionResult Get()
        {
            return Ok(usuarioServicio.Get());
        }

        [HttpPost]
        [Route("protected")]
        [Authorize]
        public async Task<ActionResult> Post([FromBody] Usuarios user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await usuarioServicio.Save(user);  // Asegúrate de esperar la tarea asincrónica
            return Ok();
        }



        [HttpPut("protected/{id}")]
        [Authorize]
        public async Task<ActionResult> Put(Guid id, [FromBody] Usuarios user)
        {
            try
            {
                await usuarioServicio.Update(id, user);  // Asegúrate de esperar la tarea asincrónica
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Usuario no encontrado" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error al actualizar el usuario", Details = ex.Message });
            }
        }


        [HttpDelete("protected/{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await usuarioServicio.Delete(id);  
                return Ok(new { Message = "Usuario eliminado correctamente" });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Usuario no encontrado" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error al eliminar el usuario", Details = ex.Message });
            }
        }
    }
}
