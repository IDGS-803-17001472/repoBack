using AuthAPI903.Data;
using AuthAPI903.Dtos;
using AuthAPI903.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AuthAPI903.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CitaController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public CitaController(UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration,
        AppDbContext context
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            this._context = context;

        }

        [Authorize]
        [HttpGet("profesional/{id}/citas")]
        public async Task<ActionResult<List<Cita>>> ObtenerCitasPorProfesional()
        {
            // Busca las citas asociadas al profesional dado

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(currentUserId!);


            var citas = await _context.Citas
                .Where(c => c.AsignacionPaciente.IdProfesional == user.Usuario.Profesional.Id)
                .Include(c => c.AsignacionPaciente)
                    .ThenInclude(ap => ap.Paciente)
                        .ThenInclude(p => p.Persona) // Incluye la información de la persona si es necesario
                .ToListAsync();

            if (citas == null || !citas.Any())
            {
                return NotFound("No se encontraron citas asociadas a este profesional.");
            }

            return Ok(citas);
        }


        [Authorize]
        [HttpGet("cita/{id}")]
        public async Task<ActionResult<Cita>> ObtenerCitaPorId(int id)
        {
            // Busca la cita por su ID y carga la información del paciente y la persona asociada
            var cita = await _context.Citas
                .Include(c => c.AsignacionPaciente)
                    .ThenInclude(ap => ap.Paciente)
                        .ThenInclude(p => p.Persona)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cita == null)
            {
                return NotFound("No se encontró la cita con el ID proporcionado.");
            }

            return Ok(cita);
        }

        [Authorize]
        [HttpPut("cita/{id}/reagendar")]
        public async Task<IActionResult> ReagendarCita(int id, [FromBody] ReagendarCitaDto nuevaCita)
        {
            // Busca la cita por su ID
            var cita = await _context.Citas.FirstOrDefaultAsync(c => c.Id == id);

            if (cita == null)
            {
                return NotFound("No se encontró la cita con el ID proporcionado.");
            }

            // Actualiza la fecha y hora de la cita
            cita.Fecha = nuevaCita.Fecha ?? cita.Fecha;
            cita.Horario = nuevaCita.Horario ?? cita.Horario;

            // Marca la cita como modificada
            _context.Citas.Update(cita);

            // Guarda los cambios en la base de datos
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("Ocurrió un problema al actualizar la cita. Intenta nuevamente.");
            }

            return Ok("La cita ha sido reagendada exitosamente.");
        }


        [Authorize]
        [HttpPut("cita/{id}/aceptar")]
        public async Task<IActionResult> AceptarCita(int id)
        {
            // Busca la cita por su ID
            var cita = await _context.Citas.FirstOrDefaultAsync(c => c.Id == id);

            if (cita == null)
            {
                return NotFound("No se encontró la cita con el ID proporcionado.");
            }

            // Verifica si la cita ya ha sido aceptada
            if (cita.Estatus == "Aceptada")
            {
                return BadRequest("La cita ya ha sido aceptada.");
            }

            // Cambia el estado de la cita a 'Aceptada'
            cita.Estatus = "Aceptada";

            // Marca la cita como modificada
            _context.Citas.Update(cita);

            // Guarda los cambios en la base de datos
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("Ocurrió un problema al aceptar la cita. Intenta nuevamente.");
            }

            return Ok("La cita ha sido aceptada exitosamente.");
        }


    }
}
