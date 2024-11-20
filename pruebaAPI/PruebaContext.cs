
using Microsoft.EntityFrameworkCore;
using pruebaAPI.Models;

namespace pruebaAPI
{
    public class PruebaContext : DbContext
    {
        public DbSet<Usuarios> User { get; set; }
        public DbSet<Empleados> Empleado { get; set; }

        public PruebaContext(DbContextOptions<PruebaContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuarios>(user =>
            {
                user.ToTable("users");
                user.HasKey(p => p.UsuariosId);
                user.Property(p => p.UsuariosId).HasColumnName("UsuariosId");
                user.Property(p => p.Name).IsRequired().HasMaxLength(150).HasColumnName("name");
                user.Property(p => p.LastName).IsRequired().HasMaxLength(150).HasColumnName("last_name");
                user.Property(p => p.MiddleName).HasMaxLength(150).HasColumnName("middle_name");
                user.Property(p => p.SecondsLastName).HasMaxLength(150).HasColumnName("seconds_last_name");
                user.Property(p => p.Age).IsRequired().HasColumnName("age");
                user.Property(p => p.Phone).HasMaxLength(20).HasColumnName("phone");
            });

            modelBuilder.Entity<Empleados>(empleado =>
            {
                empleado.ToTable("empleados");
                empleado.HasKey(e => e.EmpleadoId);
                empleado.Property(e => e.EmpleadoId).HasColumnName("EmpleadoId");
                empleado.Property(e => e.Profesional).HasMaxLength(150).HasColumnName("profesional");
                empleado.Property(e => e.Docente).HasMaxLength(150).HasColumnName("docente");
                empleado.Property(e => e.Estudiante).HasMaxLength(150).HasColumnName("estudiante");

            });

            modelBuilder.Entity<UsuarioEmpleadoDto>().HasNoKey(); // DTO sin clave
        }

    }
}
