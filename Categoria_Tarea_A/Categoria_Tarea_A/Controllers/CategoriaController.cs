using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Categoria_Tarea_A.Data;
using Categoria_Tarea_A.Models;
using Microsoft.AspNetCore.Authorization;

namespace Categoria_Tarea_A.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriaController : ControllerBase
    {
        private readonly CategoriaDBContext _dbContext;

        public CategoriaController(CategoriaDBContext dbContext)
        {
            _dbContext = dbContext;
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