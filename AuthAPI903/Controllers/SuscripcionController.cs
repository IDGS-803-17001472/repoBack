using AuthAPI903.Data;
using AuthAPI903.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI903.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SuscripcionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SuscripcionController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Suscripcion>>> ObtenerSuscripciones()
        {
            var suscripciones = await _context.Suscripciones
                .Include(s => s.Cliente)
                .Include(s => s.PlanSuscripcion)
                .Include(s => s.Pagos)
                .ToListAsync();

            if (suscripciones == null || suscripciones.Count == 0)
                return NotFound("No se encontraron suscripciones.");

            return Ok(suscripciones);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Suscripcion>> ObtenerSuscripcionPorId(int id)
        {
            var suscripcion = await _context.Suscripciones
                .Include(s => s.Cliente)
                .Include(s => s.PlanSuscripcion)
                .Include(s => s.Pagos)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (suscripcion == null)
                return NotFound("Suscripción no encontrada.");

            return Ok(suscripcion);
        }

        [HttpPost("registrar-suscripcion")]
        public async Task<ActionResult> RegistrarSuscripcion([FromBody] Suscripcion suscripcion)
        {
            _context.Suscripciones.Add(suscripcion);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Suscripción registrada exitosamente", suscripcionId = suscripcion.Id });
        }

        [HttpPut("{id}/actualizar")]
        public async Task<IActionResult> ActualizarSuscripcion(int id, [FromBody] Suscripcion suscripcion)
        {
            if (id != suscripcion.Id)
                return BadRequest("ID de suscripción no coincide.");

            _context.Entry(suscripcion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("Ocurrió un problema al actualizar la suscripción.");
            }

            return Ok("Suscripción actualizada exitosamente.");
        }

        [HttpDelete("{id}/eliminar")]
        public async Task<IActionResult> EliminarSuscripcion(int id)
        {
            var suscripcion = await _context.Suscripciones.FindAsync(id);
            if (suscripcion == null)
                return NotFound("Suscripción no encontrada.");

            _context.Suscripciones.Remove(suscripcion);
            await _context.SaveChangesAsync();

            return Ok("Suscripción eliminada exitosamente.");
        }
    }

}
