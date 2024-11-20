using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pruebaAPI.Models;
using pruebaAPI.Servicios;

namespace pruebaAPI.Controllers
{
    [Route("[controller]")]
    public class EmpleadosController : ControllerBase
    {
        IEmpleadoServicios empleadoServicio;
        private readonly ILogger<EmpleadosController> logger;

        public EmpleadosController(IEmpleadoServicios servicio, ILogger<EmpleadosController> logger)
        {
            empleadoServicio = servicio;
            this.logger = logger;
        }

        //MOSTRAR TODOS LOS EMPLEADOS 
        [HttpGet("protected")]
        [Authorize]
        public ActionResult Get()
        {
            return Ok(empleadoServicio.Get());
        }

        //MOSTRAR LA PROFESION Y EL NOMBRE DEL EMPLEADO 
        [HttpGet("profesiones")]
        [Authorize]
        public ActionResult GetProfesiones()
        {
            try
            {
                var result = empleadoServicio.GetProfesiones();
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al obtener las profesiones de los empleados");
                return StatusCode(500, $"Error al obtener las profesiones de los empleados: {ex.Message}");
            }
        }

        //PROCEDIMIENTO ALMACENADO 
        [HttpGet("almacenado")]
        public ActionResult GetProAlmacenados()
        {
            try
            {
                var empleados = empleadoServicio.GetProAlmacenado();
                return Ok(empleados);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error ejecutando el procedimiento almacenado: {ex.Message}");
            }
        }


        //CREAR UN EMPLEADO 
        [HttpPost("protected")]
        public async Task<ActionResult> Post([FromBody] Empleados empleado)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    logger.LogWarning("Modelo no válido al intentar guardar un empleado ");
                    return BadRequest(ModelState);
                }

                await empleadoServicio.Save(empleado);
                logger.LogInformation("Empleado guardado exitosamente:");

                // Devolver el objeto empleado guardado
                return Ok(new { message = "Empleado guardado exitosamente"});
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al guardar el empleado" );
                return StatusCode(500, $"Error al guardar el empleado: {ex.Message}");
            }
        }
    }
}
