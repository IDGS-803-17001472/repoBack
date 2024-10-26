using AuthAPI903.Data;
using AuthAPI903.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI903.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SuscripcionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SuscripcionController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("cliente/{idCliente}")]
        public async Task<ActionResult<List<Suscripcion>>> ObtenerSuscripcionesPorCliente(int idCliente)
        {
            var suscripciones = await _context.Suscripciones
                .Include(s => s.PlanSuscripcion)
                .Where(s => s.ClienteId == idCliente)
                .ToListAsync();

            if (suscripciones == null || !suscripciones.Any())
                return NotFound("No se encontraron suscripciones para el cliente especificado.");

            return Ok(suscripciones);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Suscripcion>> ObtenerSuscripcion(int id)
        {
            var suscripcion = await _context.Suscripciones
                .Include(s => s.Cliente)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (suscripcion == null)
                return NotFound("Suscripción no encontrada.");

            return Ok(suscripcion);
        }

        [HttpPost]
        public async Task<ActionResult> RegistrarSuscripcion([FromBody] Suscripcion suscripcion)
        {
            var suscripcionNew = new Suscripcion
            {
                ClienteId = suscripcion.ClienteId,
                PlanId = suscripcion.PlanId,
                FechaDeInicio = suscripcion.FechaDeInicio,
                FechaDeFin = suscripcion.FechaDeFin,
                Estado = "Activa"
            };

            _context.Suscripciones.Add(suscripcion);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Suscripción registrada exitosamente", suscripcionId = suscripcion.Id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarSuscripcion(int id, [FromBody] Suscripcion suscripcion)
        {
            var suscripcionNew = await _context.Suscripciones.FindAsync(id);

            if (suscripcionNew == null)
                return NotFound("Suscripción no encontrada.");

            _context.Suscripciones.Update(suscripcion);
            await _context.SaveChangesAsync();

            return Ok("La suscripción ha sido actualizada exitosamente.");
        }
    }
}
