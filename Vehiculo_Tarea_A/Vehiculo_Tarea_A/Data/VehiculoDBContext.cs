using Microsoft.EntityFrameworkCore;
using Vehiculo_Tarea_A.Models;

namespace Vehiculo_Tarea_A.Data
{
    public class VehiculoDBContext : DbContext
    {
        public VehiculoDBContext(DbContextOptions<VehiculoDBContext> options) : base(options)
        {

        }

        public DbSet<Vehiculo> vehiculos { get; set; }
    }
}
