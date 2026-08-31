using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vehiculo_Tarea_A.Data;
using Vehiculo_Tarea_A.Models;

namespace Vehiculo_Tarea_A.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiculoController : ControllerBase
    {
        private readonly VehiculoDBContext _dbContext;

        public VehiculoController(VehiculoDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vehiculo>>> GetVehiculos()
        {
            var vehiculos = await _dbContext.vehiculos
                .AsNoTracking()
                .ToListAsync();

            return Ok(vehiculos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Vehiculo>> GetVehiculo(int id)
        {
            var vehiculo = await _dbContext.vehiculos.FindAsync(id);

            if (vehiculo == null)
            {
                return NotFound();
            }

            return Ok(vehiculo);
        }

        [HttpPost]
        public async Task<ActionResult<Vehiculo>> CrearVehiculo(Vehiculo vehiculo)
        {
            _dbContext.vehiculos.Add(vehiculo);

            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVehiculo),
                new { id = vehiculo.IdVehiculo },
                vehiculo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarVehiculo(int id, Vehiculo vehiculo)
        {
            if (id != vehiculo.IdVehiculo)
            {
                return BadRequest();
            }

            _dbContext.Entry(vehiculo).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarVehiculo(int id)
        {
            var vehiculo = await _dbContext.vehiculos.FindAsync(id);

            if (vehiculo == null)
            {
                return NotFound();
            }

            _dbContext.vehiculos.Remove(vehiculo);

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}