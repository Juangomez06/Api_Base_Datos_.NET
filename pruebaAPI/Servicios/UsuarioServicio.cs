using pruebaAPI.Models;

namespace pruebaAPI.Servicios
{
    public class UsuarioServicio : IUsuarioServicio
    {
        PruebaContext context;

        //CONSTRUCTOR 
        public UsuarioServicio(PruebaContext dbcontext)
        {
            context = dbcontext;
        }

        //OBTENER
        public IEnumerable<Usuarios> Get()
        {
            return context.User;
        }

        //GUARDAR 
        public async Task Save(Usuarios user)
        {
            // Si el usuario no tiene un Id, se genera uno nuevo.
            if (user.UsuariosId == Guid.Empty)
            {
                user.UsuariosId = Guid.NewGuid();
            }

            context.Add(user);
            await context.SaveChangesAsync();
        }


        // ACTUALIZAR 
        public async Task Update(Guid id, Usuarios user)
        {
            var userActual = await context.User.FindAsync(id);  // FindAsync para respetar la asincronía
            if (userActual != null)
            {
                userActual.Name = user.Name;
                userActual.MiddleName = user.MiddleName;
                userActual.LastName = user.LastName;
                userActual.SecondsLastName = user.SecondsLastName;
                userActual.Age = user.Age;
                userActual.Phone = user.Phone;

                await context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Usuario no encontrado");
            }
        }


        // ELIMINAR  
        public async Task Delete(Guid id)
        {
            var userActual = await context.User.FindAsync(id);  
            if (userActual != null)
            {
                context.User.Remove(userActual); 
                await context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Usuario no encontrado");
            }
        }
    }

    public interface IUsuarioServicio
    {
        IEnumerable<Usuarios> Get();
        Task Save(Usuarios user);
        Task Update(Guid id, Usuarios user);
        Task Delete(Guid id);
    }
}
