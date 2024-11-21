using AuthAPI903.Data;
using AuthAPI903.Dtos;
using AuthAPI903.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI903.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PlanSuscripcionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PlanSuscripcionController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<PlanSuscripcionDto>>> ObtenerPlanes()
        {
            var planes = await _context.PlanesSuscripcion
                .Select(p => new PlanSuscripcionDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Precio = p.Precio,
                    Descripcion = p.Descripcion,
                    DuracionMeses = p.DuracionMeses
                })
                .ToListAsync();

            if (!planes.Any())
                return NotFound("No se encontraron planes de suscripción.");

            return Ok(planes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlanSuscripcionDto>> ObtenerPlanPorId(int id)
        {
            var plan = await _context.PlanesSuscripcion
                .Where(p => p.Id == id)
                .Select(p => new PlanSuscripcionDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Precio = p.Precio,
                    Descripcion = p.Descripcion,
                    DuracionMeses = p.DuracionMeses
                })
                .FirstOrDefaultAsync();

            if (plan == null)
                return NotFound("Plan de suscripción no encontrado.");

            return Ok(plan);
        }

        [HttpPost("registrar-plan")]
        public async Task<ActionResult> RegistrarPlan([FromBody] RegistrarPlanSuscripcionDto planDto)
        {
            var plan = new PlanSuscripcion
            {
                Nombre = planDto.Nombre,
                Precio = planDto.Precio,
                Descripcion = planDto.Descripcion,
                DuracionMeses = planDto.DuracionMeses
            };

            _context.PlanesSuscripcion.Add(plan);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Plan de suscripción registrado exitosamente", planId = plan.Id });
        }

        [HttpPut("{id}/actualizar")]
        public async Task<IActionResult> ActualizarPlan(int id, [FromBody] ActualizarPlanSuscripcionDto planDto)
        {
            var plan = await _context.PlanesSuscripcion.FindAsync(id);
            if (plan == null)
                return NotFound("Plan de suscripción no encontrado.");

            plan.Nombre = planDto.Nombre ?? plan.Nombre;
            plan.Precio = planDto.Precio ?? plan.Precio;
            plan.Descripcion = planDto.Descripcion ?? plan.Descripcion;
            plan.DuracionMeses = planDto.DuracionMeses ?? plan.DuracionMeses;

            _context.Entry(plan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("Ocurrió un problema al actualizar el plan de suscripción.");
            }

            return Ok("Plan de suscripción actualizado exitosamente.");
        }

        [HttpDelete("{id}/eliminar")]
        public async Task<IActionResult> EliminarPlan(int id)
        {
            var plan = await _context.PlanesSuscripcion.FindAsync(id);
            if (plan == null)
                return NotFound("Plan de suscripción no encontrado.");

            _context.PlanesSuscripcion.Remove(plan);
            await _context.SaveChangesAsync();

            return Ok("Plan de suscripción eliminado exitosamente.");
        }
    }



}
