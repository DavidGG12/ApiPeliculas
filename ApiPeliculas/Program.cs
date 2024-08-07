using ApiPeliculas.Data;
using ApiPeliculas.PeliculaMapper;
using ApiPeliculas.Repositorio;
using ApiPeliculas.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Agregamos los repositorios
builder.Services.AddScoped<ICategoriasRepositorio, CategoriasRepositorio>();
builder.Services.AddScoped<IPeliculasRepositorio, PeliculasRepositorio>();

//Agregamos el AutoMapper
builder.Services.AddAutoMapper(typeof(PeliculasMapper));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
