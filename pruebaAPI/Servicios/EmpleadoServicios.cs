using pruebaAPI.Models;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace pruebaAPI.Servicios
{
    public class EmpleadoServicios : IEmpleadoServicios
    {
        PruebaContext context;

        public EmpleadoServicios(PruebaContext dbcontext)
        {
            context = dbcontext;
        }

        //TRAER TODOS 
        public IEnumerable <Empleados> Get()
        {
            return context.Empleado.Include(t => t.Usuario).ToList();
        }

        //MOSTRAR LA PROFESION Y EL NOMBRE DEL EMPLEADO 
        public IEnumerable<object> GetProfesiones()
        {
            return context.Empleado
                .Include(e => e.Usuario) 
                .Select(e => new
                {
                    Nombre = e.Usuario.Name,
                    Apellido = e.Usuario.LastName,
                    Profesion = e.Profesional
                })
                .ToList();
        }

        //PROCEDIMIENTO ALMACENADO 
        public IEnumerable<UsuarioEmpleadoDto> GetProAlmacenado()
        {
            var resultado = context.Set<UsuarioEmpleadoDto>()
                .FromSqlRaw("EXEC ObtenerUsuariosEmpleados")
                .ToList();
            return resultado;
        }



        //GUARDAR 
        public async Task Save(Empleados empleado)
        {
            if (empleado.EmpleadoId == Guid.Empty)
                empleado.EmpleadoId = Guid.NewGuid();

            // Verifica si el empleado tiene un usuario y si el UsuarioId es vacío, generar uno
            if (empleado.Usuario != null && empleado.Usuario.UsuariosId == Guid.Empty)
                empleado.Usuario.UsuariosId = Guid.NewGuid();

            context.Add(empleado);
            await context.SaveChangesAsync();
        }

    }
    public interface IEmpleadoServicios
    {
        IEnumerable<Empleados> Get();
        IEnumerable<object> GetProfesiones();
        IEnumerable<UsuarioEmpleadoDto> GetProAlmacenado();
        Task Save(Empleados empleado);
        
    }
}
