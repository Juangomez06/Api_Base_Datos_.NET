using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace pruebaAPI.Models
{
    public class Usuarios
    {
        public Guid UsuariosId { get; set; } 
        public string Name { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string SecondsLastName { get; set; }
        public double Age { get; set; }
        public string Phone { get; set; }

        [JsonIgnore]
        public virtual ICollection<Empleados> Empleado { get; set; } = new List<Empleados>();
    }
}
