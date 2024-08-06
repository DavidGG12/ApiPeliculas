using ApiPeliculas.Modelos;
using Microsoft.EntityFrameworkCore;

/*
 * Este contexto nos ayuda a conectarnos con la base de datos,
 * sin embargo, tenemos que hacer el string en el archivo de "appsetting.json"
 * y después configuarlo para que todo se acople y se pueda conectar.
 */
namespace ApiPeliculas.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options) : base(options)
        {
        }

        //Aquí pasamos todos los modelos
        public DbSet<Categorias> Categorias { get; set; }
   
    }
}
