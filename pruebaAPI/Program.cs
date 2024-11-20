using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using pruebaAPI;
using pruebaAPI.Servicios;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Leer la configuracion de JWT desde appsettings.json
var jwtSettings = builder.Configuration.GetSection("Jwt");

// Aregar servicios al contenedor.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Configura la version y el titulo de la API en la documentacion de Swagger
    c.SwaggerDoc("v1", new() { Title = "PRUEBA BASE DE DATOS", Version = "v1" });

    // Configura la autenticacion JWT en Swagger para proteger los endpoints
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",  // Nombre del encabezado de autenticacion
        Type = SecuritySchemeType.Http,  // Tipo de esquema de seguridad
        Scheme = "bearer",  // Esquema de seguridad: Bearer (para tokens JWT)
        BearerFormat = "JWT",  // Indica que el formato es JWT
        In = ParameterLocation.Header,  // Ubicacion del token: encabezado HTTP
        Description = "Ingrese 'Bearer' [espacio] seguido de su token JWT"  // Descripcion que aparece en Swagger
    });

    // Establece los requisitos de seguridad: Swagger requiere el esquema de autenticacion "Bearer"
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,  // Referencia al esquema de seguridad
                    Id = "Bearer"  // Nombre del esquema definido anteriormente
                }
            },
            new string[] {}  // No se especifican roles o scopes, aplicable para cualquier endpoint seguro
        }
    });
});

// Configuracion del esquema de autenticacion JWT
builder.Services.AddAuthentication(options =>
{
    // Esquema predeterminado para autenticar las solicitudes
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    // Esquema predeterminado para manejar los desafios de autenticacion
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Configuracion adicional para el esquema JWT
.AddJwtBearer(options =>
{
    // Par�metros de validacion del token JWT
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,  // Valida que el emisor (Issuer) del token sea correcto
        ValidateAudience = true,  // Valida que la audiencia (Audience) del token sea correcta
        ValidateLifetime = true,  // Valida que el token no este expirado
        ValidateIssuerSigningKey = true,  // Valida la clave de firma del token

        // Emisor y audiencia validos, que se obtienen de la configuracion (jwtSettings)
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        // Clave de firma del token, convertida a bytes
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
    };
});

//conexion base de datos 
builder.Services.AddSqlServer<PruebaContext>(
    "Data Source=localhost;Database=pruebas;User Id=sa;Password=123456789;TrustServerCertificate=True"
);

//Instanciamos a la clase 
builder.Services.AddScoped<IUsuarioServicio, UsuarioServicio>();
builder.Services.AddScoped<IEmpleadoServicios, EmpleadoServicios>();

var app = builder.Build();

//Obtner logger 
var logger = app.Services.GetRequiredService<ILogger<Program>>();

// Verificación de conexión a la base de datos
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PruebaContext>();
    try
    {
        // Intentar abrir la conexión para verificar si es exitosa
        dbContext.Database.CanConnect();
        logger.LogInformation("------ Conexión a la base de datos exitosa. -----");
    }
    catch (Exception ex)
    {
        logger.LogWarning($" ---- Error al conectar a la base de datos: {ex.Message} ---- ");
    }
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();


