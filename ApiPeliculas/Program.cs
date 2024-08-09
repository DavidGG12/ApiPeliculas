using ApiPeliculas.Data;
using ApiPeliculas.PeliculaMapper;
using ApiPeliculas.Repositorio;
using ApiPeliculas.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
/*
 * Aquí es donde tendremos que especificar tanto el contexto como la cadena de conexión que vamos a usar.
 * 
 * Con esto nos podremos conectar a la base de datos sin problema, ahora solo quedaría hacer nuestros modelos
 * lo cual consiste en crear las estructuras de la tabla que queremos crear en la base de datos cuando queramos hacer la migración.
 * 
 * OJO: Esta API crea su base de datos desde aquí, por eso es que haremos el modelo y después haremos la migración.
 */
builder.Services.AddDbContext<ApplicationDbContext>(opciones =>
    opciones.UseSqlServer(builder.Configuration.GetConnectionString("conexion")));

//Soporte para caché
builder.Services.AddResponseCaching();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = 
        "Autenticación JWT usando el esquema Bearer. \r\n\r\n " +
        "Ingresa la palabra Bearer seguido de un [espacio] y después su token en el campo de abajo \r\n\r\n" +
        "Ejemplo: \"Bearer tkljk125jhhk\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement() 
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

/*
 * Soporte para CORS
 * Los CORS son permisos para acceder al servicio web desde otro dominio,
 * porque automáticamente al tratar de acceder a un servicio web
 * desde otro dominio, este se bloquea, así que para eso necesitamos los CORS
 * 
 * Se pueden habilitar: 1.Un dominio, 2.Múltiples dominios, 3. Cualquier dominio
 */

builder.Services.AddCors(p => p.AddPolicy("PoliticaCors", build =>
{
    /*Esta línea quiere decir que el CORS se puede usar en la ruta: https://localhost:7159/,
    * el cual puede usar todos los métodos gracias a que le colocamos "AllowAnyMethod()" 
    */
    build.WithOrigins("https://localhost:7159").AllowAnyMethod().AllowAnyHeader();
}));

//Agregamos los repositorios
builder.Services.AddScoped<ICategoriasRepositorio, CategoriasRepositorio>();
builder.Services.AddScoped<IPeliculasRepositorio, PeliculasRepositorio>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

var key = builder.Configuration.GetValue<string>("ApiSettings:Secreta");

//Agregamos el AutoMapper
builder.Services.AddAutoMapper(typeof(PeliculasMapper));

//Configuración de la autenticación
builder.Services.AddAuthentication(
        x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;            
        }
).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Soporte para CORS
app.UseCors("PoliticaCors");

//Soporte para Autenticación
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
