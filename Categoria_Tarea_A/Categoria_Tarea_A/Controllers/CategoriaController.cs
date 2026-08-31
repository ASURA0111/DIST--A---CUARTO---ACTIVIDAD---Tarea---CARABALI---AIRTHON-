using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Categoria_Tarea_A.Data;
using Categoria_Tarea_A.Models;
using Categoria_Tarea_A.Services; // Importación agregada para el servicio de RabbitMQ

namespace Categoria_Tarea_A.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly CategoriaDBContext _dbContext;

        private readonly RabbitMQPublisher _rabbitMQPublisher;

        // 2. Inyección de dependencias en el constructor
        public CategoriaController(
            CategoriaDBContext dbContext,
            RabbitMQPublisher rabbitMQPublisher)
        {
            _dbContext = dbContext;
            _rabbitMQPublisher = rabbitMQPublisher;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategorias()
        {
            var categorias = await _dbContext.categorias
                .AsNoTracking()
                .ToListAsync();

            return Ok(categorias);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Categoria>> GetCategoria(int id)
        {
            var categoria = await _dbContext.categorias.FindAsync(id);

            if (categoria == null)
            {
                return NotFound();
            }

            return Ok(categoria);
        }

        [HttpPost]
        public async Task<ActionResult<Categoria>> CrearCategoria(Categoria categoria)
        {
            _dbContext.categorias.Add(categoria);

            await _dbContext.SaveChangesAsync();

            // 3. Llamada al publicador de RabbitMQ tras guardar en la base de datos
            await _rabbitMQPublisher.PublicarCategoriaCreadaAsync(categoria);

            return CreatedAtAction(nameof(GetCategoria),
                new { id = categoria.IdCategoria },
                categoria);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarCategoria(int id, Categoria categoria)
        {
            if (id != categoria.IdCategoria)
            {
                return BadRequest();
            }

            _dbContext.Entry(categoria).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarCategoria(int id)
        {
            var categoria = await _dbContext.categorias.FindAsync(id);

            if (categoria == null)
            {
                return NotFound();
            }

            _dbContext.categorias.Remove(categoria);

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}