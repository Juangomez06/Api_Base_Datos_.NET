using System;
using System.Text.Json.Serialization;

namespace pruebaAPI.Models
{
    public class Empleados
    {
        public Guid EmpleadoId { get; set; }
        public Guid UsuarioId { get; set; }
        public string Profesional { get; set; }
        public string Docente { get; set; }
        public string Estudiante { get; set; }
        public virtual Usuarios? Usuario { get; set; }
    }
}
