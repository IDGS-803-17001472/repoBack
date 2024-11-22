using AuthAPI903.Data;
using AuthAPI903.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI903.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuejaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public QuejaController(AppDbContext context)
        {
            _context = context;
        }

        // API para registrar una nueva queja
        [HttpPost("registrar")]
        public async Task<ActionResult<Queja>> RegisterQueja(Queja queja)
        {
            // Añadir la queja a la base de datos
            _context.Quejas.Add(queja);

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            // Retornar la respuesta con el nuevo id autogenerado
            return CreatedAtAction(nameof(GetQuejas), new { id = queja.Id }, queja);
        }

        // API para traer todas las quejas
        [HttpGet("todas")]
        public async Task<ActionResult<IEnumerable<Queja>>> GetQuejas()
        {
            return await _context.Quejas.ToListAsync();
        }

        // API para actualizar una queja (cambiar descripción, tipo, estatus)
        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> UpdateQueja(int id, [FromBody] Queja queja)
        {
            if (id != queja.Id) return BadRequest();

            var quejaExistente = await _context.Quejas.FindAsync(id);
            if (quejaExistente == null) return NotFound();

            quejaExistente.Descripcion = queja.Descripcion;
            quejaExistente.Tipo = queja.Tipo;
            quejaExistente.Estatus = queja.Estatus;

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }


}

