using Categoria_Tarea_A.Models;
using Microsoft.EntityFrameworkCore;

namespace Categoria_Tarea_A.Data
{
    public class CategoriaDBContext : DbContext
    {
        public CategoriaDBContext(DbContextOptions<CategoriaDBContext> options) : base(options)
        {

        }

        public DbSet<Categoria> categorias { get; set; }
    }
}
