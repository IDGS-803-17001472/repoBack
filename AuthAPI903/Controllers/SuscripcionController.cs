using AuthAPI903.Data;
using AuthAPI903.Dtos;
using AuthAPI903.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI903.Controllers
{
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
        public async Task<ActionResult<List<SuscripcionDto>>> ObtenerSuscripciones()
        {
            var suscripciones = await _context.Suscripciones
                .Include(s => s.Cliente)
                .Include(s => s.PlanSuscripcion)
                .Include(s => s.Pagos)
                .Select(s => new SuscripcionDto
                {
                    Id = s.Id,
                    FechaDeInicio = s.FechaDeInicio,
                    FechaDeFin = s.FechaDeFin,
                    Estado = s.Estado,
                    ClienteId = s.ClienteId,
                    PlanId = s.PlanId,
                    PagoIds = s.Pagos.Select(p => p.Id).ToList()
                })
                .ToListAsync();

            if (suscripciones == null || suscripciones.Count == 0)
                return NotFound("No se encontraron suscripciones.");

            return Ok(suscripciones);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SuscripcionDto>> ObtenerSuscripcionPorId(int id)
        {
            var suscripcion = await _context.Suscripciones
                .Include(s => s.Cliente)
                .Include(s => s.PlanSuscripcion)
                .Include(s => s.Pagos)
                .Where(s => s.Id == id)
                .Select(s => new SuscripcionDto
                {
                    Id = s.Id,
                    FechaDeInicio = s.FechaDeInicio,
                    FechaDeFin = s.FechaDeFin,
                    Estado = s.Estado,
                    ClienteId = s.ClienteId,
                    PlanId = s.PlanId,
                    PagoIds = s.Pagos.Select(p => p.Id).ToList()
                })
                .FirstOrDefaultAsync();

            if (suscripcion == null)
                return NotFound("Suscripción no encontrada.");

            return Ok(suscripcion);
        }

        [HttpPost("registrar-suscripcion")]
        public async Task<ActionResult> RegistrarSuscripcion([FromBody] RegistrarSuscripcionDto suscripcionDto)
        {
            var suscripcion = new Suscripcion
            {
                FechaDeInicio = suscripcionDto.FechaDeInicio,
                FechaDeFin = suscripcionDto.FechaDeFin,
                Estado = suscripcionDto.Estado,
                ClienteId = suscripcionDto.ClienteId,
                PlanId = suscripcionDto.PlanId
            };

            _context.Suscripciones.Add(suscripcion);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Suscripción registrada exitosamente", suscripcionId = suscripcion.Id });
        }

        [HttpPut("{id}/actualizar")]
        public async Task<IActionResult> ActualizarSuscripcion(int id, [FromBody] ActualizarSuscripcionDto suscripcionDto)
        {
            var suscripcion = await _context.Suscripciones.FindAsync(id);
            if (suscripcion == null)
                return NotFound("Suscripción no encontrada.");

            suscripcion.FechaDeInicio = suscripcionDto.FechaDeInicio;
            suscripcion.FechaDeFin = suscripcionDto.FechaDeFin;
            suscripcion.Estado = suscripcionDto.Estado;
            suscripcion.ClienteId = suscripcionDto.ClienteId;
            suscripcion.PlanId = suscripcionDto.PlanId;

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
